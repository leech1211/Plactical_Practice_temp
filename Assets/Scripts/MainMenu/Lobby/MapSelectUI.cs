using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapSelectUI : MonoBehaviour
{
    [SerializeField] Button changeMapButton;
    [SerializeField] private List<Button> maps;
    [SerializeField] Transform mapSelectUI_;

    [Header("Selected Color")]
    [SerializeField] ColorBlock selectedColor;
    [Header("UnSelected Color")]
    [SerializeField] ColorBlock unSelectedColor;

    private MapList targetMap;
    
    private void Start()
    {
        foreach (var mapButton in maps)
        {
            mapButton.onClick.AddListener(() => { OnClickMapButton(mapButton); });
            changeMapButton.onClick.AddListener(ChangeMap);
        }
    }

    private void OnClickMapButton(Button mapButton)
    {
        foreach (var button in maps)
        {
            if (mapButton == button)
            {
                button.colors = selectedColor;
                targetMap = button.GetComponent<MapSlot>().Map;
                continue;
            }

            button.colors = unSelectedColor;
        }
        
        mapSelectUI_.SetParent(mapButton.transform);
        mapSelectUI_.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 19);
    }
    
    private async void ChangeMap()
    {
        bool succeded = await GameLobbyManager.instance.SetMap(targetMap);
        if(succeded == true)
            gameObject.SetActive(false);
    }
}