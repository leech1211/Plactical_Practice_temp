using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnetor : MonoBehaviour
{
    public static NodeConnetor instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetTargetObject(string fieldName)
    {
        Transform targetField = transform.Find(fieldName);
        if (targetField == null)
            return null;
        return targetField.gameObject;
    }
}