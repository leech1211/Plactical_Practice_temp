using System;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

        NetworkManager.Singleton.LogLevel = LogLevel.Developer;
        NetworkManager.Singleton.NetworkConfig.EnableNetworkLogs = true;
        
        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
        if (RelayManager.instance.IsHost)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApproval;
            (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) =
                RelayManager.instance.GetHostConnectionInfo();
            NetworkManager.Singleton.GetComponent<UnityTransport>()
                .SetHostRelayData(ip, (ushort)port, allocationId, key, connectionData, true);
        }
        else
        {
            (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) =
                RelayManager.instance.GetClientConnectionInfo();
            NetworkManager.Singleton.GetComponent<UnityTransport>()
                .SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
        }
    }

    private void OnClientConnected(ulong obj)
    {
        Debug.Log($"Player Connected: {obj}");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            Debug.Log($"You're Disconnected: {clientId}");
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }

    private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;
        response.Pending = false;
    }
}
