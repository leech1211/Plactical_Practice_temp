using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    [SerializeField] private InventoryManager iManager;
    
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image icon;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        iManager.SaveAndExit();
    }

    public void SetActive()
    {
        button.interactable = true;
        text.color = Color.white;
        icon.color = Color.white;
    }
    
    public void SetInactive()
    {
        button.interactable = false;
        text.color = new Color(1f, 1f, 1f, 0.6f);
        icon.color = text.color;

    }
}