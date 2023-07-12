using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainMenu;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UserLobby;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance = null;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private Lobby _lobby;
    private Coroutine _heartbeatCoroutine;
    private Coroutine _refreshLobbyCoroutine;
    private List<string> _joinedLobbiesId;

    public async Task<bool> HasActiveLobbies()
    {
        try
        {
            _joinedLobbiesId = await LobbyService.Instance.GetJoinedLobbiesAsync();
            if (_joinedLobbiesId.Count > 0)
            {
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public void SetActiveLobbies(string id)
    {
        _joinedLobbiesId = new List<string>() { id };
    }

    public async Task<bool> CreateLobby(Dictionary<string, string> data, Dictionary<string, string> lobbyData, int maxPlayer)
    {
        Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
        Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

        CreateLobbyOptions options = new CreateLobbyOptions()
        {
            Data = SerializeLobbyData(lobbyData),
            IsPrivate = true,
            Player = player,
        };

        try
        {
            _lobby = await LobbyService.Instance.CreateLobbyAsync("Lobby", maxPlayer, options);
        }
        catch (System.Exception)
        {
            return false;
        }

        _heartbeatCoroutine = StartCoroutine(HeartbeatLobbyCoroutine(_lobby.Id, 6f));
        _refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(_lobby.Id, 1.1f));
        
        return true;
    }

    public async Task<bool> JoinLobby(string inviteCode, Dictionary<string, string> playerData)
    {
        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
        Player player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(playerData));

        options.Player = player;

        try
        {
            _lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(inviteCode, options);
        }
        catch (System.Exception)
        {
            return false;
        }

        _refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(_lobby.Id, 1.1f));
        
        return true;
    }

    private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTime)
    {
        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private IEnumerator RefreshLobbyCoroutine(string lobbyId, float waitTime)
    {
        while (true)
        {
            Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyId);
            yield return task;
            while (task.IsCompleted == false)
            {
                yield return null;
            }

            Lobby newLobby = task.Result;

            _lobby = newLobby;
            LobbyEvents.OnLobbyUpdated?.Invoke(_lobby);
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
        foreach (var (key, value) in data)
        {
            playerData.Add(key, new PlayerDataObject(
                visibility: PlayerDataObject.VisibilityOptions.Member,
                value: value));
        }

        return playerData;
    }

    private Dictionary<string, DataObject> SerializeLobbyData(Dictionary<string, string> data)
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();
        foreach (var (key, value) in data)
        {
            lobbyData.Add(key, new DataObject(
                visibility: DataObject.VisibilityOptions.Member,
                value: value));
        }

        return lobbyData;
    }

    public void OnApplicationQuit()
    {
        if (_lobby != null && _lobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            LobbyService.Instance.DeleteLobbyAsync(_lobby.Id);
        }
    }

    public string GetLobbyCode()
    {
        return _lobby?.LobbyCode;
    }

    public List<Player> GetPlayersData()
    {
        return _lobby?.Players;
    }

    public async Task<bool> UpdatePlayerData(Player localPlayerData, string allocationId = default, string connectionData = default)
    {
        UpdatePlayerOptions options = new UpdatePlayerOptions()
        {
            Data = localPlayerData.Data,
            AllocationId = allocationId,
            ConnectionInfo = connectionData
        };
        try
        {
            await LobbyService.Instance.UpdatePlayerAsync(_lobby.Id, localPlayerData.Id, options);
        }
        catch(System.Exception)
        {
            return false;
        }

        LobbyEvents.OnLobbyUpdated(_lobby);

        return true;
    }

    public async Task<bool> UpdateLobbyData(Dictionary<string, string> data)
    {
        Dictionary<string, DataObject> lobbyData = SerializeLobbyData(data);

        UpdateLobbyOptions options = new UpdateLobbyOptions()
        {
            Data = lobbyData
        };
        try
        {
            await LobbyService.Instance.UpdateLobbyAsync(_lobby.Id, options);
        }
        catch(System.Exception)
        {
            return false;
        }

        LobbyEvents.OnLobbyUpdated(_lobby);

        return true;
    }

    public bool IsHost()
    {
        return _lobby != null && _lobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async Task<bool> RejoinLobby()
    {
        try
        {
            _lobby = await LobbyService.Instance.ReconnectToLobbyAsync(_joinedLobbiesId[0]);
            LobbyEvents.OnLobbyUpdated(_lobby);
        }
        catch (System.Exception)
        {
            return false;
        }
        
        _refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(_joinedLobbiesId[0], 1.1f));
        return true;
    }

    public async Task<bool> LeaveAllLobby()
    {
        string playerId = AuthenticationService.Instance.PlayerId;
        StopAllCoroutines();
        foreach (string lobbyId in _joinedLobbiesId)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<bool> LeaveCurrentLobby()
    {
        string playerId = AuthenticationService.Instance.PlayerId;
        StopAllCoroutines();
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_lobby.Id, playerId);
            _lobby = null;
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public void InitializeLobby()
    {
        StopAllCoroutines();
        _lobby = null;
    }
}
