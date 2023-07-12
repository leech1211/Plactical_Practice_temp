using System.Collections.Generic;
using UnityEngine;

public class Function2_1 : Node_Skill
{
    public override object RunCode(int outputIndex)
    {
        if (!CompileCountCheck())
        {
            return 0f;
        }
        
        if (GameNetWorkManager.instance.isNodeCount)
        {
            if(!GameNetWorkManager.instance.usedNodes.Contains(_nodeType))
                GameNetWorkManager.instance.usedNodes.Add(_nodeType);
        }

        return null;
    }
}