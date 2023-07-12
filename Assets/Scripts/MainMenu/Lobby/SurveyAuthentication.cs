using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SurveyAuthentication : MonoBehaviour
{
    [SerializeField] private Button summitButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button closeButton2;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject errorText;
    [SerializeField] private GameObject PopUp_Authentication;
    [SerializeField] private GameObject PopUp_SelectServey;

    private GoogleData GD;
    private const string URL =
        "https://script.google.com/macros/s/AKfycbw7I19teyZYg-PJSI7aBkHWMiyv2ghFl4aA67NzNmIC7jOAIcxOujpRR8Sr2zU6EPUgQA/exec";

    private void OnEnable()
    {
        errorText.SetActive(false);
        PopUp_Authentication.SetActive(true);
        PopUp_SelectServey.SetActive(false);
        inputField.text = "";
        summitButton.interactable = true;
    }

    private void Start()
    {
        summitButton.onClick.AddListener(OnSummitClicked);
        closeButton.onClick.AddListener(OnCloseClicked);
        closeButton2.onClick.AddListener(OnCloseClicked);
    }

    private void OnCloseClicked()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void OnSummitClicked()
    {
        TryLoginStage("SurveyCode", inputField.text);
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
            Login();
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

    private void Login()
    {
        Debug.Log("로그인 성공");
        PopUp_Authentication.SetActive(false);
        PopUp_SelectServey.SetActive(true);
    }
    
    private void LoginFailed()
    {
        summitButton.interactable = true;
        errorText.SetActive(true);
    }

    public void Servey_Pre()
    {
        gameObject.SetActive(false);
        Application.OpenURL("https://moaform.com/q/umNKvL/");
    }
    
    public void Servey_Post()
    {
        gameObject.SetActive(false);
        Application.OpenURL("https://moaform.com/q/8L5moV/");
    }
}
