using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserLobby
{
    public class MainPopUpUI : MonoBehaviour
    {
        [SerializeField] private Button joinButton;
        [SerializeField] private Button popUpCloseButton;

        [SerializeField] private GameObject PopUp;

        private void Awake()
        {
            joinButton.onClick.AddListener(() =>
            {
                PopUp.SetActive(true);
            });
            popUpCloseButton.onClick.AddListener(() =>
            {
                PopUp.SetActive(false);
            });
        }
    }
}