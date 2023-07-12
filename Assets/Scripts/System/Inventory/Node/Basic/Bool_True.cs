using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bool_True : Node
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
        
        return true;
    }
}