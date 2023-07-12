using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using UserLobby;

public class GameNetWorkManager : NetworkBehaviour
{
    public static GameNetWorkManager instance = null;

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

    [SerializeField] private AudioClip MainSound;
    [SerializeField] private AudioClip BossSound;

    public int playerCount;
    public int currPlayerCount;

    public NetworkVariable<float> time = new NetworkVariable<float>(0);
    public NetworkVariable<int> coding = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public bool isNodeCount;
    public List<NodeType> usedNodes;

    public Vector3 respawnPosition;
    
    private NetworkVariable<bool> isStart = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private AudioSource mainAudio;

    public NetworkVariable<int> monsterKillCount = new NetworkVariable<int>(0);
    
    public int BonusmonsterCurrCount;

    private void Start()
    {
        mainAudio = SoundManager.instance.SFXPlayLoop("Main",MainSound, 0.1f);
        isNodeCount = false;
        if (IsHost)
        {
            isStart.Value = false;
        }
    }
}
