using System.Collections.Generic;
using UnityEngine;

public class Operator_Multiply_Int : Node
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
        
        int result = 0, a = 0, b = 0;

        if (inputButton[0] != null)
        {
            a = (int)inputButton[0].GetComponent<LineManager>().RunCode();
        }

        if (inputButton[1] != null)
        {
            b = (int)inputButton[1].GetComponent<LineManager>().RunCode();
        }

        return a * b;
    }
}