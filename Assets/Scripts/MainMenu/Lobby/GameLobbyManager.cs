using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MainMenu;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UserLobby;

public class GameLobbyManager : MonoBehaviour
{
    public static GameLobbyManager instance = null;

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

    [SerializeField] private string gameSceneName;
    [SerializeField] private List<LobbyPlayerSingleUI> _lobbyPlayerDatas;
    private Player _localPlayerData;
    private Dictionary<string, DataObject> _lobbyData;
    private MapList _currentMap;
    private int _playerCount;
    private bool _isStarted;
    private bool _isRejoin;
    private string _previousLobbyId;

    public MapList CurrentMap
    {
        get
        {
            return _currentMap;
        }
    }

    private void OnEnable()
    {
        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

    private void OnDisable()
    {
        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }

    public void InitializeLobbyData(List<LobbyPlayerSingleUI> players)
    {
        LobbyManager.instance.InitializeLobby();
        _lobbyPlayerDatas = players;
        _isStarted = false;
        _isRejoin = false;
    }

    public async Task<bool> CreateLobby(MapList map, int maxPlayer)
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"PlayerName", PlayerPrefs.GetString("UserName")},
            {"IsReady", "T"},
        };
        Dictionary<string, string> lobbyData = new Dictionary<string, string>()
        {
            { "RelayCode", default },
            { "Map", ((int)map).ToString() }
        };
        bool succeeded = await LobbyManager.instance.CreateLobby(playerData, lobbyData, maxPlayer);
        return succeeded;
    }

    public async Task<bool> JoinLobby(string inviteCode)
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"PlayerName", PlayerPrefs.GetString("UserName")},
            {"IsReady", "F"},
        };

        bool succeeded = await LobbyManager.instance.JoinLobby(inviteCode, playerData);
        return succeeded;
    }

    public string GetInviteCode()
    {
        return LobbyManager.instance.GetLobbyCode();
    }

    private async void OnLobbyUpdated(Lobby lobby)
    {
        if (_isStarted == true)
            return;
        
        int readyCount = 0;
        _playerCount = lobby.Players.Count;
        
        for (int i = 0; i < _lobbyPlayerDatas.Count; i++)
        {
            if (i < _playerCount && _lobbyPlayerDatas[i] != null)
            {
                if(lobby.Players[i].Data["IsReady"].Value == "T")
                {
                    readyCount++;
                }
                _lobbyPlayerDatas[i].UpdatePlayer(lobby.Players[i]);
                _lobbyPlayerDatas[i].gameObject.SetActive(true);
                if(lobby.Players[i].Id == AuthenticationService.Instance.PlayerId)
                {
                    _localPlayerData = lobby.Players[i];
                }
            }
            else
            {
                _lobbyPlayerDatas[i].gameObject.SetActive(false);
            }
        }

        _lobbyData = lobby.Data;
        _currentMap = (MapList)int.Parse(_lobbyData["Map"].Value);
        
        
        if (readyCount == _playerCount)
        {
            LobbyEvents.OnLobbyReady?.Invoke();
        }
        else
        {
            LobbyEvents.OnLobbyUnReady?.Invoke();
        }

        if (_lobbyData["RelayCode"].Value != default && !_isStarted)
        {
            _previousLobbyId = lobby.Id;
            MainMenuController.instance.SetGameStartLoadingOn(_currentMap.ToString());
            bool succeded = await JoinRelayServer(_lobbyData["RelayCode"].Value);
            if (succeded)
            {
                StartCoroutine(StartGameClient(_isRejoin));
            }
        }
    }

    private IEnumerator StartGameClient(bool isRejoin)
    {
        if (!isRejoin)
        {
            if (ItemStorage.instance != null)
            {
                Destroy(ItemStorage.instance.gameObject);
            }

            if (InventoryField.instance != null)
            {
                Destroy(InventoryField.instance.gameObject);
            }
        }
        
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(_currentMap.ToString());
        while(sceneLoading.isDone == false)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        GameNetWorkManager.instance.playerCount = _playerCount;
    }

    public async Task<bool> SetPlayerReady()
    {
        _localPlayerData.Data["IsReady"].Value = "T";
        bool succeeded = await LobbyManager.instance.UpdatePlayerData(_localPlayerData);
        return succeeded;
    }

    public async Task<bool> SetPlayerUnReady()
    {
        _localPlayerData.Data["IsReady"].Value = "F";
        bool succeeded = await LobbyManager.instance.UpdatePlayerData(_localPlayerData);
        return succeeded;
    }
    
    public async Task<bool> SetMap(MapList targetMap)
    {
        Dictionary<string, string> lobbyData = new Dictionary<string, string>()
        {
            { "RelayCode", default },
            { "Map", ((int)targetMap).ToString() }
        };
        bool succeeded = await LobbyManager.instance.UpdateLobbyData(lobbyData);
        if (succeeded)
            _currentMap = targetMap;
        return succeeded;
    }

    public async Task<bool> SetPlayerName(string playerNameText)
    {
        _localPlayerData.Data["PlayerName"].Value = playerNameText;
        bool succeeded = await LobbyManager.instance.UpdatePlayerData(_localPlayerData);
        return succeeded;
    }

    public async Task StartGame()
    {
        _isStarted = true;
        string relayCode = await RelayManager.instance.CreateRelay(_playerCount);
        Dictionary<string, string> lobbyData = new Dictionary<string, string>()
        {
            { "RelayCode", relayCode },
            { "Map", ((int)_currentMap).ToString() }
        };
        await LobbyManager.instance.UpdateLobbyData(lobbyData);
        
        string allocationId = RelayManager.instance.GetAllocationId();
        string connectionData = RelayManager.instance.GetConnedtionData();
        _localPlayerData.Data["IsReady"].Value = "F";
        await LobbyManager.instance.UpdatePlayerData(_localPlayerData, allocationId, connectionData);
        StartCoroutine(StartGameClient(_isRejoin));
    }

    private async Task<bool> JoinRelayServer(string relayCode)
    {
        _isStarted = true;
        try
        {
            await RelayManager.instance.JoinRelay(relayCode);
            string allocationId = RelayManager.instance.GetAllocationId();
            string connectionData = RelayManager.instance.GetConnedtionData();
            _localPlayerData.Data["IsReady"].Value = "F";
            await LobbyManager.instance.UpdatePlayerData(_localPlayerData, allocationId, connectionData);
        }
        catch (Exception e)
        {
            MainMenuController.instance.SetMessage("유효하지 않은 방", "존재하지 않은 방입니다.");
            return false;
        }

        return true;
    }
    
    public bool IsHost()
    {
        return LobbyManager.instance.IsHost();
    }

    public async Task<bool> HasActiveLobbies()
    {
        bool succeeded = await LobbyManager.instance.HasActiveLobbies();
        if (succeeded)
        {
            return true;
        }
        
        if (_previousLobbyId != default && !RelayManager.instance.IsHost)
        {
            LobbyManager.instance.SetActiveLobbies(_previousLobbyId);
            return true;
        }

        return false;
    }

    public async Task<bool> RejoinGame()
    {
        bool succeeded = await LobbyManager.instance.RejoinLobby();
        if (succeeded)
        {
            _isRejoin = true;
        }
        return succeeded;
    }

    public async Task<bool> LeaveAllLobby()
    {
        bool succeeded = await LobbyManager.instance.LeaveAllLobby();
        return succeeded;
    }

    public async Task<bool> LeaveLobby()
    {
        bool succeeded = await LobbyManager.instance.LeaveCurrentLobby();
        return succeeded;
    }

    // public async void GoBackToLobby(bool wasDisconnected)
    // {
    //     _isStarted = false;
    //     _wasDisconnected = wasDisconnected;
    //
    //     if (_wasDisconnected)
    //     {
    //         _previousRelayCode = _lobbyData["RelayCode"].Value;
    //     }
    //     
    //     _localPlayerData.Data["IsReady"].Value = "F";
    //     await LobbyManagerV2.instance.UpdatePlayerData(_localPlayerData);
    //     StartCoroutine(BackToLobby());
    // }
    //
    // public IEnumerator BackToLobby()
    // {
    //     SceneManager.LoadScene("MainMenu");
    //     yield return new WaitForEndOfFrame();
    //     MainMenuController.instance.BackToLobby();
    // }
}
