
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopUpUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text message;

    [SerializeField] private Button okButton;

    private void Awake()
    {
        okButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            MainMenuController.instance.ResetUI();
        });
    }

    public void SetMessage(string title, string message)
    {
        gameObject.SetActive(true);
        this.title.text = title;
        this.message.text = message;
    }
}
