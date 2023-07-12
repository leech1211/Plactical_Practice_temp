using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance { get; set; }

    private void Awake()
    {
        instance = this;
    }
    
    [Header("MainUI")]
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private GameObject _mainPopUpUI;
    [SerializeField] private GameObject _surveyPopUpUI;
    [SerializeField] private Button _selectMapButton;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _rejoinButton;
    [SerializeField] private Button _sandboxModeButton;
    [SerializeField] private Button _surveyButton;

    [SerializeField] private TMP_InputField _inviteCode;
    [SerializeField] private TMP_Text _versionText;
    [SerializeField] private AudioClip MainSound;

    [Header("LobbyUI")]
    [SerializeField] private MainLobbyUI _lobbyUI;

    [Header("MapSelectUI")]
    public MapSelectUIManager _mapSelectUI;
    
    [Header("LoadingUI")]
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private GameObject _gameStartLoadingUI;

    [Header("MessagePopUpUI")]
    [SerializeField] private MessagePopUpUI _messagePopUpUI;

    async void Start()
    {
        SoundManager.instance.SFXPlayLoop("mainBGM", MainSound, 0.1f);
        _versionText.text = $"Version: {Application.version}";
        _selectMapButton.onClick.AddListener(OnSelectMapClicked);
        _joinButton.onClick.AddListener(OnJoinClicked);
        _sandboxModeButton.onClick.AddListener(OnSandboxModeClicked);
        
        GameLobbyManager.instance.InitializeLobbyData(_lobbyUI.GetPlayers());

        if (await GameLobbyManager.instance.HasActiveLobbies() && !RelayManager.instance.IsHost)
        {
            _rejoinButton.gameObject.SetActive(true);
            _rejoinButton.onClick.AddListener(OnRejoinGameClicked);
        }
    }

    private void OnSurveyClicked()
    {
        _surveyPopUpUI.SetActive(true);
    }

    private void OnSelectMapClicked()
    {
        _mainUI.SetActive(false);
        _mainPopUpUI.SetActive(false);
        _mapSelectUI.gameObject.SetActive(true);
    }

    public async void StartHost(MapList map, int maxPlayer)
    {
        SetLoadingOn();
        bool succeeded = await GameLobbyManager.instance.CreateLobby(map, maxPlayer);
        if (succeeded)
        {
            _mainUI.SetActive(false);
            _mainPopUpUI.SetActive(false);
            _mapSelectUI.Reset();
            _mapSelectUI.gameObject.SetActive(false);
            _lobbyUI.gameObject.SetActive(true);
            _lobbyUI.SetHostUI();
            SetLoadingOff();
        }
        else
        {
            _messagePopUpUI.SetMessage("방 과다생성", "현재 생성되는 방이 너무 많습니다.\n잠시 후에 다시 시도해주세요.");
        }
    }

    private async void OnJoinClicked()
    {
        if (_inviteCode.text.Equals(""))
        {
            return;
        }
        SetLoadingOn();
        bool succeeded = await GameLobbyManager.instance.JoinLobby(_inviteCode.text);
        if(succeeded){
            _mainUI.SetActive(false);
            _mainPopUpUI.SetActive(false);
            _lobbyUI.gameObject.SetActive(true);
            _lobbyUI.SetClientUI();
            SetLoadingOff();
        }
        else
        {
            _messagePopUpUI.SetMessage("유효하지 않은 방", "존재하지 않은 방입니다.");
        }
    }

    private async void OnRejoinGameClicked()
    {
        SetLoadingOn();
        bool succeeded = await GameLobbyManager.instance.RejoinGame();
        if (succeeded)
        {
            _mainUI.SetActive(false);
            _mainPopUpUI.SetActive(false);
            _lobbyUI.gameObject.SetActive(true);
            _lobbyUI.SetClientUI();
        }
        else
        {
            _messagePopUpUI.SetMessage("유효하지 않은 게임", "존재하지 않은 게임입니다.");
        }
    }

    private void OnSandboxModeClicked()
    {
        _mainUI.SetActive(false);
        StartCoroutine(SandBoxMode());
    }

    private IEnumerator SandBoxMode()
    {
        _mainUI.SetActive(false);
        SetLoadingOn();
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        InventoryField.instance.SetSandBoxMode();
        SetLoadingOff();
    }

    public void ResetUI()
    {
        _mainUI.SetActive(true);
        _mainPopUpUI.SetActive(false);
        _lobbyUI.gameObject.SetActive(false);
        _mapSelectUI.Reset();
        _mapSelectUI.gameObject.SetActive(false);
        _gameStartLoadingUI.SetActive(false);
        _lobbyUI.ResetLobbyUI();
        SetLoadingOff();
    }

    public void SetLoadingOn()
    {
        _loadingUI.SetActive(true);
    }
    
    public void SetLoadingOff()
    {
        _loadingUI.SetActive(false);
    }
    
    public void SetGameStartLoadingOn(string SceneName)
    {
        string[] imgNames = SceneName.Split("_");
        _gameStartLoadingUI.transform.GetChild(0).GetComponent<Image>().sprite =
            Resources.Load($"Image/LoadingUI/{imgNames[0]}_{imgNames[1]}", typeof(Sprite)) as Sprite;
        _gameStartLoadingUI.SetActive(true);
    }
    
    public void SetGameStartLoadingOff()
    {
        _gameStartLoadingUI.SetActive(false);
    }
    
    public void SetMessage(string title, string message)
    {
        _messagePopUpUI.SetMessage(title, message);
    }
}
