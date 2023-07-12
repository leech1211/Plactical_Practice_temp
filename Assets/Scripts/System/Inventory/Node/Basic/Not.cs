using System.Collections.Generic;
using UnityEngine;

public class Not : Node
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

        if (inputButton[0] != null)
        {
            bool a = (bool)inputButton[0].GetComponent<LineManager>().RunCode();
            return !a;
        }
        
        return null;
    }
}