using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    [SerializeField] private MapList targetStage;

    private StageAuthentication authenticationPopup;
    private TMP_Text stageTitle;
    private Button button;

    private void Start()
    {
        authenticationPopup = MainMenuController.instance._mapSelectUI.authenticationPopup;
        stageTitle = transform.GetChild(1).GetComponent<TMP_Text>();
        button = transform.GetChild(0).GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        authenticationPopup.Initialize(targetStage, stageTitle.text);
    }
}
