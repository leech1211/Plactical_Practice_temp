using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    public static ItemStorage instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Fields = new Dictionary<string, GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Dictionary<string, GameObject> Fields;
    [HideInInspector] public Transform InventoryUI;

    private void StoreItemField()
    {
        string currName = InventoryField.instance.currentFieldName;
        Transform currTransform = InventoryField.instance.transform;
        Transform destTransform;
        if (!Fields.TryGetValue(currName, out var tempField))
        {
            CreateField(currName);
        }

        int childrenCount = currTransform.childCount;
        destTransform = Fields[currName].transform;
        for (int i = 0; i < childrenCount; i++)
        {
            currTransform.GetChild(0).parent = destTransform;
        }
    }

    private void CreateField(string fieldName)
    {
        GameObject newField = new GameObject(fieldName);
        newField.transform.position = new Vector3(960f, 540f, -201f);
        newField.transform.parent = transform;
        Fields.Add(fieldName, newField);
        newField.SetActive(false);
    }

    public void ChangeField(string fieldName)
    {
        if (fieldName.Equals(InventoryField.instance.currentFieldName))
        {
            return;
        }
        
        if (Fields.TryGetValue(fieldName, out var tempField))
        {
            StoreItemField();
            
            Transform tempFieldTransform = tempField.transform;
            Transform destTransform = InventoryField.instance.transform;

            int childrenCount = tempFieldTransform.childCount;
            
            for (int i = 0; i < childrenCount; i++)
            {
                tempFieldTransform.GetChild(0).parent = destTransform;
            }

            if (fieldName.Equals("Inventory"))
            {
                InventoryField.instance.fieldState = FieldState.Inventory;
                InventoryUI.GetChild(0).gameObject.SetActive(true);
                InventoryUI.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(170f, 100f);
                InventoryUI.GetChild(1).gameObject.SetActive(false);
                InventoryUI.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170f, 100f);
            }
            else if (fieldName.Contains("Puzzle"))
            {
                InventoryField.instance.fieldState = FieldState.Skill;
                InventoryUI.GetChild(2).gameObject.SetActive(true);
                InventoryUI.GetChild(0).gameObject.SetActive(true);
                InventoryUI.GetChild(1).gameObject.SetActive(false);
                InventoryUI.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170f, 100f);
            }
            else if (fieldName.Contains("SandBox"))
            {
                InventoryField.instance.fieldState = FieldState.SandBox;
                InventoryUI.GetChild(0).gameObject.SetActive(true);
                InventoryUI.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(170f, 100f);
                InventoryUI.GetChild(1).gameObject.SetActive(false);
                InventoryUI.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170f, 100f);
            }
            else
            {
                InventoryField.instance.fieldState = FieldState.Skill;
                InventoryUI.GetChild(0).gameObject.SetActive(false);
                InventoryUI.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170f, 100f);
                InventoryUI.GetChild(1).gameObject.SetActive(true);
                InventoryUI.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(170f, 100f);
            }

            ItemListUI.instance.InitializeItems();
            InventoryField.instance.currentFieldName = fieldName;
            
            GameObject.Find("InventoryCamera").GetComponent<InventoryCameraManager>().ResetCamrera();
        }
        else
        {
            Debug.Log($"Can't find {fieldName}");
        }
    }

    public void ChangeFieldForPuzzle(string fieldName, bool firstField)
    {
        if (fieldName.Equals(InventoryField.instance.currentFieldName))
        {
            return;
        }

        if (Fields.TryGetValue(fieldName, out var tempField))
        {
            InventoryFieldPreset tempPreset = tempField.GetComponent<InventoryFieldPreset>();
            StoreItemField();

            Transform tempFieldTransform = tempField.transform;
            Transform destTransform = InventoryField.instance.transform;

            int childrenCount = tempFieldTransform.childCount;

            for (int i = 0; i < childrenCount; i++)
            {
                tempFieldTransform.GetChild(0).parent = destTransform;
            }
            
            InventoryField.instance.fieldState = FieldState.Quiz;

            ItemListUI.instance.InitializeItemsByPreset(tempPreset.PresetItems, tempPreset.GetAllowedInventoryItems(), tempPreset.isPresetMode, tempPreset.isInfinityMode);
            InventoryField.instance.currentFieldName = fieldName;

            GameObject.Find("InventoryCamera").GetComponent<InventoryCameraManager>().ResetCamrera();
        }
        else
        {
            Debug.Log($"Can't find {fieldName}");
        }

        if (!firstField)
        {
            InventoryUI.GetChild(0).gameObject.SetActive(false);
            InventoryUI.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170f, 100f);
            InventoryUI.GetChild(1).gameObject.SetActive(true);
            InventoryUI.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(170f, 100f);
        }
        else
        {
            InventoryUI.GetChild(0).gameObject.SetActive(true);
            InventoryUI.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(170f, 100f);
            InventoryUI.GetChild(1).gameObject.SetActive(false);
            InventoryUI.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170f, 100f);
        }
    }

    public void AddField(string fieldName, GameObject field)
    {
        Fields.Add(fieldName, field);
        field.SetActive(false);
    }

    public Transform GetField(string fieldName)
    {
        return Fields[fieldName].transform;
    }
    
    public bool HasField(string fieldName)
    {
        if (Fields.ContainsKey(fieldName))
        {
            return true;
        }
        return false;
    }

    public void SetFieldPreset(string fieldName, GameObject obj)
    {
        Fields.Add(fieldName, obj);
        obj.SetActive(false);
    }
    
    public void ChangeBackButton(string fieldName)
    {
        BackButton.instance.ChangeFieldName(fieldName, false);
    }
}