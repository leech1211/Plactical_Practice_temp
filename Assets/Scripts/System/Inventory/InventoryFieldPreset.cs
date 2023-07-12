using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFieldPreset : MonoBehaviour
{
    [SerializeField] private string fieldName;
    public GameObject ResultNode; //old코드 아마 아래 코드로 교체될 가능성이 있음
    
    [Header("PresetItems")]
    public bool isPresetMode;
    [SerializeField] public List<string> presetItems_Key;
    [SerializeField] public List<int> presetItems_Value;
    [SerializeField] public List<string> allowedInventoryItems_Key;

    [Header("InfinityMode")]
    public bool isInfinityMode;

    public Dictionary<NodeType, int> PresetItems;

    private void Awake()
    {
        PresetItems = GetPresetItems();
    }

    void Start()
    {
        ItemStorage.instance.SetFieldPreset(fieldName, gameObject);
    }

    public Dictionary<NodeType, int> GetPresetItems()
    {
        Dictionary<NodeType, int> presetItems = new Dictionary<NodeType, int>();
        for (int i = 0; i < presetItems_Key.Count; i++)
        {
            presetItems.Add((NodeType)Enum.Parse(typeof(NodeType) ,presetItems_Key[i]), presetItems_Value[i]);
        }

        return presetItems;
    }
    
    public List<NodeType> GetAllowedInventoryItems()
    {
        List<NodeType> allowedInventoryItems = new List<NodeType>();
        for (int i = 0; i < allowedInventoryItems_Key.Count; i++)
        {
            allowedInventoryItems.Add((NodeType)Enum.Parse(typeof(NodeType) ,allowedInventoryItems_Key[i]));
        }

        return allowedInventoryItems;
    }

    public void UpdatePresetItems()
    {
        PresetItems = GetPresetItems();
    }
}