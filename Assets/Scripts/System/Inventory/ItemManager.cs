using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance = null;

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
    }
    
    private Dictionary<NodeType, int> items;
    private Dictionary<NodeType, int> presetItems;
    public List<NodeType> allowedInventoryItems;

    private void Start()
    {
        items = new Dictionary<NodeType, int>();
    }

    public void AddItem(NodeType node)
    {
        if (items.TryGetValue(node, out int count))
        {
            items[node] = count + 1;
        }
        else
        {
            items.Add(node, 1);
        }
    }

    public void AddItem(NodeType node, bool isPreset)
    {
        if (!isPreset)
        {
            if (items.TryGetValue(node, out int count))
            {
                items[node] = count + 1;
            }
            else
            {
                items.Add(node, 1);
            }
        }
        else
        {
            if (allowedInventoryItems != null)
            {
                if (allowedInventoryItems.Contains(node))
                {
                    if (items.TryGetValue(node, out int count))
                    {
                        items[node] = count + 1;
                    }
                    else
                    {
                        items.Add(node, 1);
                    }

                    return;
                }
            }

            if (presetItems.TryGetValue(node, out int count2))
            {
                presetItems[node] = count2 + 1;
            }
            else
            {
                presetItems.Add(node, 1);
            }
        }
    }

    public void RemoveItem(NodeType node, bool isPreset)
    {
        if (!isPreset)
        {
            if (items.TryGetValue(node, out int count))
            {
                if (count == 1)
                {
                    items.Remove(node);
                }
                else
                {
                    items[node] = count - 1;
                }
            }
        }
        else
        {
            if (allowedInventoryItems != null)
            {
                if (allowedInventoryItems.Contains(node))
                {
                    if (items.TryGetValue(node, out int count))
                    {
                        if (count == 1)
                        {
                            items.Remove(node);
                        }
                        else
                        {
                            items[node] = count - 1;
                        }

                        return;
                    }
                }
            }

            if (presetItems.TryGetValue(node, out int count2))
            {
                if (count2 == 1)
                {
                    presetItems.Remove(node);
                }
                else
                {
                    presetItems[node] = count2 - 1;
                }
            }
        }
    }

    public void ResetItem()
    {
        items.Clear();
    }

    public Dictionary<NodeType, int> GetItems()
    {
        return items;
    }
    
    public void SetPresetItems(Dictionary<NodeType, int> newItems, List<NodeType> allowedItems)
    {
        presetItems = newItems;
        allowedInventoryItems = allowedItems;
    }
}
