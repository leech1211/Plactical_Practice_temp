using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class GoogleData
{
    public string order, result, msg;
}

public class StageAuthentication : MonoBehaviour
{
    [SerializeField] private Button summitButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private GameObject errorText;

    [SerializeField] private MapList targetStage;
    
    private GoogleData GD;
    private const string URL =
        "https://script.google.com/macros/s/AKfycbw7I19teyZYg-PJSI7aBkHWMiyv2ghFl4aA67NzNmIC7jOAIcxOujpRR8Sr2zU6EPUgQA/exec";

    private void OnEnable()
    {
        errorText.SetActive(false);
        inputField.text = "";
        summitButton.interactable = true;
    }

    private void OnDisable()
    {
        targetStage = MapList.Null;
    }
    
    private void Start()
    {
        summitButton.onClick.AddListener(OnSummitClicked);
        closeButton.onClick.AddListener(OnCloseClicked);
    }

    public void Initialize(MapList newStage, string title)
    {
        targetStage = newStage;
        titleText.text = title;
        gameObject.SetActive(true);
    }

    private void OnCloseClicked()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void OnSummitClicked()
    {
        TryLoginStage(targetStage.ToString(), inputField.text);
    }

    public void TryLoginStage(string stageId, string password)
    {
        if (stageId.Trim() == "")
        {
            return;
        }

        if (password.Trim() == "")
        {
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", stageId);
        form.AddField("pw", password);

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        MapList map = targetStage;
        summitButton.interactable = false;
        errorText.SetActive(false);
        bool isSucceeded;
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
                isSucceeded = Response(www.downloadHandler.text);
            else
            {
                Debug.Log("웹의 응답이 없습니다");
                LoginFailed();
                yield break;
            }

            if (!isSucceeded)
            {
                LoginFailed();
                yield break;
            }
            if(map == MapList.Stage_Week4_1 || map == MapList.Stage_Week4_2)
                Login(map,2);
            else
                Login(map,6);
        }
    }

    private bool Response(string json)
    {
        if (string.IsNullOrEmpty(json))
            return false;
        GD = JsonUtility.FromJson<GoogleData>(json);

        if (GD.result == "ERROR")
            return false;
        
        return true;
    }

    private void Login(MapList map, int maxPlayer)
    {
        Debug.Log("로그인 성공");
        gameObject.SetActive(false);
        MainMenuController.instance.StartHost(map, maxPlayer);
    }
    
    private void LoginFailed()
    {
        summitButton.interactable = true;
        errorText.SetActive(true);
    }
}
