using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryField : MonoBehaviour
{
    public static InventoryField instance = null;
    public short compileCounter;

    public SkillSlot1 skillSlot1;
    public SkillSlot2 skillSlot2;
    public SkillSlot3 skillSlot3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        compileCounter = 0;
    }

    public FieldState fieldState = FieldState.Inventory;
    public string currentFieldName = "Inventory";

    public void Initialize()
    {
        fieldState = FieldState.Inventory;
        ItemStorage.instance.ChangeField("Inventory");
    }

    public void SetSandBoxMode()
    {
        GameObject.Find("StartCamera").GetComponent<AudioListener>().enabled = false;
        Transform UI = GameObject.Find("InventoryUI").transform;
        
        ItemStorage.instance.ChangeField("SandBox");
        UI.Find("Coding").gameObject.SetActive(false);
        UI.GetChild(0).gameObject.SetActive(true);
        UI.GetChild(1).gameObject.SetActive(false);
        UI.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(170f, 100f);
        UI.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170f, 100f);
        BackButton.instance.ChangeFieldName("SandBox", false);
        for (int i = 0; i < 3; i++)
        {
            ItemManager.instance.AddItem(NodeType.FLOAT_1);
            ItemManager.instance.AddItem(NodeType.FLOAT_2);
            ItemManager.instance.AddItem(NodeType.FLOAT_3);
            ItemManager.instance.AddItem(NodeType.FLOAT_M1);
            ItemManager.instance.AddItem(NodeType.FLOAT_M2);
            ItemManager.instance.AddItem(NodeType.FLOAT_M3);
            ItemManager.instance.AddItem(NodeType.OPERATOR_PLUS);
            ItemManager.instance.AddItem(NodeType.OPERATOR_MINUS);
            ItemManager.instance.AddItem(NodeType.OPERATOR_MULTIPLY);
            ItemManager.instance.AddItem(NodeType.INT_2);
            ItemManager.instance.AddItem(NodeType.INT_3);
        }

        ItemListUI.instance.InitializeItems();
    }

    public void ExitInventory()
    {
        if(InventoryManager.instance != null)
            InventoryManager.instance.SaveAndExit();
    }
}
