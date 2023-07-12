using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UserLobby;

public class RelayManager : MonoBehaviour
{
    public static RelayManager instance = null;

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

    public bool IsHost
    {
        get { return _isHost; }
    }

    private bool _isHost = false;
    private string _joinCode;
    private string _ip;
    private int _port;
    private byte[] _key;
    private byte[] _connectionData;
    private byte[] _hostConnectionData;
    private System.Guid _allocationId;
    private byte[] _allocationIdBytes;

    public async Task<string> CreateRelay(int playerCount)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(playerCount);

            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            // RelayServerData serverData = new RelayServerData(allocation, "dtls");
            // NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            //
            // NetworkManager.Singleton.StartHost();
            
            RelayServerEndpoint dltsEndpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
            
            _ip = dltsEndpoint.Host;
            _port = dltsEndpoint.Port;
            
            _allocationId = allocation.AllocationId;
            _allocationIdBytes = allocation.AllocationIdBytes;
            _connectionData = allocation.ConnectionData;
            _key = allocation.Key;

            _isHost = true;
            
            return _joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return "0";
        }
    }

    public async Task<bool> JoinRelay(string joinCode)
    {
        try
        {
            _joinCode = joinCode;
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            // RelayServerData serverData = new RelayServerData(joinAllocation, "dtls");
            // NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            //
            //NetworkManager.Singleton.StartClient();
            
            RelayServerEndpoint dltsEndpoint = allocation.ServerEndpoints.First(conn => conn.ConnectionType == "dtls");
            _ip = dltsEndpoint.Host;
            _port = dltsEndpoint.Port;
            
            _allocationId = allocation.AllocationId;
            _allocationIdBytes = allocation.AllocationIdBytes;
            _connectionData = allocation.ConnectionData;
            _hostConnectionData = allocation.HostConnectionData;
            _key = allocation.Key;

            _isHost = false;

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public string GetAllocationId()
    {
        return _allocationId.ToString();
    }

    public string GetConnedtionData()
    {
        return _connectionData.ToString();
    }

    public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, string _dtlsAddress, int _dtlsPort) GetHostConnectionInfo()
    {
        return (_allocationIdBytes, _key, _connectionData, _ip, _port);
    }
    
    public (byte[] AllocationId, byte[] Key, byte[] ConnectionData, byte[] HostConnectionData, string _dtlsAddress, int _dtlsPort) GetClientConnectionInfo()
    {
        return (_allocationIdBytes, _key, _connectionData, _hostConnectionData, _ip, _port);
    }
}