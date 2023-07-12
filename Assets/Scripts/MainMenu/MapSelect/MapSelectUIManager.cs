using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapStage
{
    Main = 0,
    Stage_Short,
    Stage_Week1,
    Stage_Week2,
    Stage_Week3,
    Stage_Week4,
    Stage_Week5,
}

public class MapSelectUIManager : MonoBehaviour
{
    [SerializeField] private Transform field;
    [SerializeField] private Transform stagesContaner;
    [SerializeField] private Button backButton;
    [SerializeField] private ScrollRect scrollRect;
    public StageAuthentication authenticationPopup;

    private MapStage currentMapStage;

    private void Start()
    {
        backButton.onClick.AddListener(back);
    }

    public void ChangeField(MapStage newStage)
    {
        if(currentMapStage == newStage)
            return;
        
        Transform parentContaner = stagesContaner.GetChild((int)currentMapStage);
        int count = field.childCount;
        
        for(int i = 0; i < count; i++)
            field.GetChild(0).SetParent(parentContaner);

        parentContaner = stagesContaner.GetChild((int)newStage);
        count = stagesContaner.GetChild((int)newStage).childCount;
        
        for(int i = 0; i < count; i++)
            parentContaner.GetChild(0).SetParent(field);

        scrollRect.horizontalNormalizedPosition = 0;
        
        currentMapStage = newStage;
    }
    
    private void back()
    {
        if (currentMapStage == MapStage.Main)
        {
            MainMenuController.instance.ResetUI();
        }
        else
            ChangeField(MapStage.Main);
    }

    public void Reset()
    {
        authenticationPopup.gameObject.SetActive(false);
        ChangeField(MapStage.Main);
    }
}