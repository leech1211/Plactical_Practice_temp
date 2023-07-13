using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour, IPointerClickHandler
{
    public static BackButton instance;
    
    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    
    [SerializeField] private string FieldName;
    [SerializeField] private bool IsPuzzleField;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!IsPuzzleField)
            ItemStorage.instance.ChangeField(FieldName);
        else
            ItemStorage.instance.ChangeFieldForPuzzle(FieldName, true);
    }

    public void ChangeFieldName(string fieldName, bool isPuzzleField)
    {
        FieldName = fieldName;
        IsPuzzleField = isPuzzleField;
    }

    
}
