using System;
using System.Collections;
using System.Collections.Generic;
using MainMenu;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using UserLobby;

public class MainLobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject Lobby_Name_POPUP;
    [SerializeField] private GameObject Lobby_Map_POPUP;
        
    [SerializeField] private List<LobbyPlayerSingleUI> players;

    [SerializeField] private TMP_Text inviteCode;
    [SerializeField] private TMP_Text mapName;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private Button changeNameButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button unPlayButton;
    [SerializeField] private Button ReadyButton;
    [SerializeField] private Button unReadyButton;
    [SerializeField] private Button leaveLobbyButton;
    [SerializeField] private GameObject namePopupButton;
    [SerializeField] private GameObject mapPopupButton;

    // Start is called before the first frame update
    private void OnEnable()
    {
        changeNameButton.onClick.AddListener(ChangeName);
        unReadyButton.onClick.AddListener(Ready);
        ReadyButton.onClick.AddListener(UnReady);
        leaveLobbyButton.onClick.AddListener(Leave);
        inviteCode.text = $"Invite Code: {GameLobbyManager.instance.GetInviteCode()}";
        LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        
        if (GameLobbyManager.instance.IsHost())
        {
            playButton.onClick.AddListener(StartGame);
            LobbyEvents.OnLobbyReady += OnLobbyReady;
            LobbyEvents.OnLobbyUnReady += OnLobbyUnReady;
        }
    }

    private void OnDisable()
    {
        changeNameButton.onClick.RemoveAllListeners();
        unReadyButton.onClick.RemoveAllListeners();
        ReadyButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
        leaveLobbyButton.onClick.RemoveAllListeners();
        LobbyEvents.OnLobbyReady -= OnLobbyReady;
        LobbyEvents.OnLobbyUnReady -= OnLobbyUnReady;
        LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }

    private void OnLobbyUpdated(Lobby lobby)
    {
        MapListKR mapIndex = (MapListKR)int.Parse(lobby.Data["Map"].Value);
        mapName.text = $"Map - {mapIndex.ToString()}";
    }

    public void SetHostUI()
    {
        unReadyButton.gameObject.SetActive(false);
        //mapPopupButton.gameObject.SetActive(true);
    }

    public void SetClientUI()
    {
        unPlayButton.gameObject.SetActive(false);
        //mapPopupButton.gameObject.SetActive(false);
    }

    private async void StartGame()
    {
        MainMenuController.instance.SetGameStartLoadingOn(GameLobbyManager.instance.CurrentMap.ToString());
        await GameLobbyManager.instance.StartGame();
    }

    private async void UnReady()
    {
        bool succeeded = await GameLobbyManager.instance.SetPlayerUnReady();
        if (succeeded)
        {
            unReadyButton.gameObject.SetActive(true);
            ReadyButton.gameObject.SetActive(false);
            namePopupButton.gameObject.SetActive(true);
        }
    }

    private async void Ready()
    {
        bool succeeded = await GameLobbyManager.instance.SetPlayerReady();
        if (succeeded)
        {
            unReadyButton.gameObject.SetActive(false);
            ReadyButton.gameObject.SetActive(true);
            namePopupButton.gameObject.SetActive(false);
        }
    }

    private async void ChangeName()
    {
        bool succeeded = await GameLobbyManager.instance.SetPlayerName(playerName.text);
        if (succeeded)
        {
            PlayerPrefs.SetString("UserName", playerName.text);
            Lobby_Name_POPUP.SetActive(false);
        }
    }

    private void OnLobbyReady()
    {
        playButton.gameObject.SetActive(true);
    }

    private void OnLobbyUnReady()
    {
        playButton.gameObject.SetActive(false);
    }

    private async void Leave()
    {
        MainMenuController.instance.SetLoadingOn();
        bool succeded = await GameLobbyManager.instance.LeaveLobby();
        if (succeded)
        {
            foreach (var playerUI in players)
            {
                playerUI.gameObject.SetActive(false);
            }
            MainMenuController.instance.ResetUI();
        }
        else
        {
            MainMenuController.instance.SetMessage("오류", "알수없는 오류가 발생했습니다.");
        }
    }
    
    public List<LobbyPlayerSingleUI> GetPlayers()
    {
        return players;
    }

    public void ResetLobbyUI()
    {
        inviteCode.text = "Invite Code: ";
        playButton.gameObject.SetActive(false);
        unPlayButton.gameObject.SetActive(true);
        ReadyButton.gameObject.SetActive(false);
        unReadyButton.gameObject.SetActive(true);
        namePopupButton.SetActive(true);
    }
}
