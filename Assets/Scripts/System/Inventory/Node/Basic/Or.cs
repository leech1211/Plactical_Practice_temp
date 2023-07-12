﻿using System.Collections.Generic;
using UnityEngine;

public class Or : Node
{
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

        if (inputButton[0] != null && inputButton[1] != null)
        {
            bool a = (bool)inputButton[0].GetComponent<LineManager>().RunCode();
            bool b = (bool)inputButton[1].GetComponent<LineManager>().RunCode();
            return a || b;
        }
        
        return null;
    }
}