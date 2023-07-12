using System.Collections.Generic;
using UnityEngine;

public class GraterEqual : Node
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
            float a = (float)inputButton[0].GetComponent<LineManager>().RunCode();
            float b = (float)inputButton[1].GetComponent<LineManager>().RunCode();
            return a >= b;
        }
        
        return null;
    }
}