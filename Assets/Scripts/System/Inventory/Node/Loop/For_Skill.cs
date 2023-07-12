using System.Collections.Generic;
using UnityEngine;

public class For_Skill : Node
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

        if (outputFlow[0] == null)
            return null;
        
        if (inputButton[0] == null)
            return null;
        
        Node targetSKill = outputFlow[0].transform.parent.parent.GetComponent<Node>();
        int count = (int)inputButton[0].GetComponent<LineManager>().RunCode();
        float result = 0f;

        for (int i = 0; i < count; i++)
        {
            result += (float)targetSKill.RunCode(0);
        }

        return result * 0.5f;
    }
}