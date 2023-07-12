using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UserLobby
{
    public class LobbyPlayerSingleUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Image readyImage;
        //[SerializeField] private Button kickPlayerButton;

        private Player player;
        public string isReady;

        // private void Awake()
        // {
        //     if(kickPlayerButton != null)
        //     kickPlayerButton.onClick.AddListener(KickPlayer);
        // }
        
        // public void SetKickPlayerButtonVisible(bool visible) {
        //     kickPlayerButton.gameObject.SetActive(visible);
        // }
        
        public void UpdatePlayer(Player player) {
            this.player = player;
            playerNameText.text = player.Data["PlayerName"].Value;
            isReady = player.Data["IsReady"].Value;
            if (isReady.Equals("T"))
            {
                readyImage.enabled = true;
            }
            else
            {
                readyImage.enabled = false;
            }
        }
        
        // private void KickPlayer() {
        //     if (player != null) {
        //         LobbyManager_Old.instance.KickPlayer(player.Id);
        //     }
        // }
    }
}