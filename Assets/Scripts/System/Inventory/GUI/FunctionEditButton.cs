using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FunctionEditButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string FieldName;
    [SerializeField] private string BackName;
    [SerializeField] private List<GameObject> Preset;
    [SerializeField] private List<Vector3> PresetPosition;

    private bool isInitialize;
    
    private void OnEnable()
    {
        isInitialize = false;
    }

    private void Update()
    {
        if (isInitialize == false)
        {
            if (!ItemStorage.instance.HasField(FieldName))
            {
                GameObject newField = new GameObject();
                newField.name = FieldName;
                newField.transform.position = new Vector3(960f, 540f, -201f);
                newField.transform.parent = ItemStorage.instance.transform;

                int i = 0;
                foreach (var presetNode in Preset)
                {
                    GameObject tempNode = Instantiate(presetNode);
                    tempNode.transform.parent = newField.transform;
                    tempNode.GetComponent<RectTransform>().anchoredPosition3D = PresetPosition[i];

                    if (i == 0)
                    {
                        transform.parent.parent.GetComponent<Node_Skill>().propertyObject = tempNode;
                    }
                    
                    i++;
                }
                ItemStorage.instance.AddField(FieldName, newField);
            }
            else if(Preset.Count != 0)
            {
                transform.parent.parent.GetComponent<Node_Skill>().propertyObject =
                    ItemStorage.instance.GetField(FieldName).GetChild(0).gameObject;
            }

            isInitialize = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemStorage.instance.ChangeField(FieldName);
        ItemStorage.instance.ChangeBackButton(BackName);
    }
}
