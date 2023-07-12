using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitalizeMain : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            AuthenticationService.Instance.SignedIn += InstanceOnSignedIn;
            
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (AuthenticationService.Instance.IsSignedIn)
            {
                string userName = PlayerPrefs.GetString("UserName");
                if (userName == "")
                {
                    userName = "Player" + Random.Range(1000,9999);
                    PlayerPrefs.SetString("UserName", userName);
                }

                SceneManager.LoadSceneAsync(mainMenuSceneName);
            }
        }
        
        Application.targetFrameRate = 60;
    }

    private void InstanceOnSignedIn()
    {
        Debug.Log($"Player ID : {AuthenticationService.Instance.PlayerId}");
        Debug.Log($"Token : {AuthenticationService.Instance.AccessToken}");
    }
}
