using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemListUI : MonoBehaviour
{
    public static ItemListUI instance;

    private void Awake()
    {
        instance = this;
    }

    public bool isInfinityMode;
    public bool isPresetMode;

    private void Start()
    {
        InitializeItems();
    }

    public void InitializeItems()
    {
        isPresetMode = false;
        Draggable_Item slot;
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            slot = transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Draggable_Item>();
            slot.ResetItemSlot();
        }

        var items = ItemManager.instance.GetItems();
        GameObject NodePrefab;

        #region itemSetting

        foreach (var item in items)
        {
            NodeType type = item.Key;
            NodePrefab = Resources.Load("Node/" + type.ToString()) as GameObject;

            for (int i = 0; i < item.Value; i++)
            {
                SetItem(NodePrefab);
            }
        }
        #endregion
    }

    public void InitializeItemsByPreset(Dictionary<NodeType, int> presetItems, List<NodeType> allowedItems, bool presetMode, bool infinityMode)
    {
        isPresetMode = presetMode;
        isInfinityMode = infinityMode;

        Draggable_Item slot;
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            slot = transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Draggable_Item>();
            slot.ResetItemSlot();
        }

        GameObject NodePrefab;
        Dictionary<NodeType, int> items;

        if (isPresetMode)
        {
            if(presetItems == null)
                return;
            items = presetItems;
            ItemManager.instance.SetPresetItems(items, allowedItems);
            
            foreach (var item in items)
            {
                NodeType type = item.Key;
                NodePrefab = Resources.Load("Node/" + type.ToString()) as GameObject;

                for (int i = 0; i < item.Value; i++)
                    SetItem(NodePrefab);
            }
            
            if(allowedItems == null)
                return;
            
            items = ItemManager.instance.GetItems();

            foreach (var item in items)
            {
                NodeType type = item.Key;

                if (!allowedItems.Contains(type))
                    continue;

                NodePrefab = Resources.Load("Node/" + type.ToString()) as GameObject;

                for (int i = 0; i < item.Value; i++)
                    SetItem(NodePrefab);
            }

            return;
        }
        
        items = ItemManager.instance.GetItems();

        foreach (var item in items)
        {
            NodeType type = item.Key;

            NodePrefab = Resources.Load("Node/" + type.ToString()) as GameObject;

            for (int i = 0; i < item.Value; i++)
                SetItem(NodePrefab);
        }
    }

    private void SetItem(GameObject item)
    {
        Draggable_Item slot = null;
        Node itemNode = item.GetComponent<Node>();
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            slot = transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Draggable_Item>();
            if (slot.nodeObject != null)
            {
                if (slot.nodeObject.GetComponent<Node>().NODE_TYPE == itemNode.NODE_TYPE)
                {
                    int count = int.Parse(slot.Count.text);
                    slot.Count.GetComponentInParent<Image>().enabled = true;
                    slot.Count.enabled = true;
                    slot.Count.text = $"{(count + 1)}";
                    return;
                }
            }
        }
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            slot = transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Draggable_Item>();
            if (slot.nodeObject == null)
            {
                slot.nodeObject = Resources.Load(itemNode.PrefebPath) as GameObject;
                slot.image.enabled = true;
                slot.image.sprite = itemNode.Icon;
                slot.title.text = itemNode.Title;
                slot.info.text = itemNode.Information;
                slot.Count.text = "1";
                return;
            }
        }
    }

    public bool AddItem(GameObject item)
    {
        if (isInfinityMode)
        {
            if (!ItemManager.instance.allowedInventoryItems.Contains((NodeType)Enum.Parse(typeof(NodeType),
                    item.GetComponent<Node>().NODE_TYPE)))
                return false;
        }

        Draggable_Item slot = null;
        Node itemNode = item.GetComponent<Node>();
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            slot = transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Draggable_Item>();
            if (slot.nodeObject != null)
            {
                if (slot.nodeObject.GetComponent<Node>().NODE_TYPE == itemNode.NODE_TYPE)
                {
                    int count = int.Parse(slot.Count.text);
                    slot.Count.GetComponentInParent<Image>().enabled = true;
                    slot.Count.enabled = true;
                    slot.Count.text = $"{(count + 1)}";
                    ItemManager.instance.AddItem((NodeType)Enum.Parse(typeof(NodeType), itemNode.NODE_TYPE) , isPresetMode);
                    return true;
                }
            }
        }
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            slot = transform.GetChild(0).GetChild(i).GetChild(2).GetComponent<Draggable_Item>();
            if (slot.nodeObject == null)
            {
                slot.nodeObject = Resources.Load(itemNode.PrefebPath) as GameObject;
                slot.image.enabled = true;
                slot.image.sprite = itemNode.Icon;
                slot.title.text = itemNode.Title;
                slot.info.text = itemNode.Information;
                slot.Count.text = "1";
                ItemManager.instance.AddItem((NodeType)Enum.Parse(typeof(NodeType), itemNode.NODE_TYPE), isPresetMode);
                return true;
            }
        }
        return false;
    }
}
