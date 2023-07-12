using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapStageButton : MonoBehaviour
{
    private MapSelectUIManager mapSelectUIManager;
    [SerializeField] private Button button;
    [SerializeField] private MapStage targetStage;
    
    private void Start()
    {
        mapSelectUIManager = MainMenuController.instance._mapSelectUI;
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        mapSelectUIManager.ChangeField(targetStage);
    }
}
