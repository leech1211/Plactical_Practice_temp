using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UserLobby
{
    public class LobbyPopUpUI : MonoBehaviour
    {
        [Header("Change Name")]
        [SerializeField] private Button changeNameButton;
        [SerializeField] private Button popUpCloseButton;
        [SerializeField] private TMP_InputField PlayerName;

        [SerializeField] private GameObject NamePopUp;
        
        [Header("Change Map")]
        [SerializeField] private Button changeMapButton;
        [SerializeField] private Button popUpCloseButton2;

        [SerializeField] private GameObject MapPopUp;

        private void Awake()
        {
            changeNameButton.onClick.AddListener(() =>
            {
                PlayerName.text = PlayerPrefs.GetString("UserName");
                NamePopUp.SetActive(true);
            });
            popUpCloseButton.onClick.AddListener(() =>
            {
                NamePopUp.SetActive(false);
            });
            
            changeMapButton.onClick.AddListener(() =>
            {
                MapPopUp.SetActive(true);
            });
            popUpCloseButton2.onClick.AddListener(() =>
            {
                MapPopUp.SetActive(false);
            });
        }
    }
}