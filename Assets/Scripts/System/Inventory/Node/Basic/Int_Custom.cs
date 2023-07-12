using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Int_Custom : Node
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText;
    
    public override object RunCode(int outputIndex)
    {
        if (!CompileCountCheck())
        {
            return null;
        }
        
        if (GameNetWorkManager.instance.isNodeCount)
        {
            if(!GameNetWorkManager.instance.usedNodes.Contains(_nodeType))
                GameNetWorkManager.instance.usedNodes.Add(_nodeType);
        }

        return (int)slider.value;
    }

    private void Update()
    {
        valueText.text = slider.value.ToString();
    }
}