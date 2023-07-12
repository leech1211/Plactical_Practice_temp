using System.Collections.Generic;
using UnityEngine;

public class Function1_2 : Node_Skill
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

        if (propertyObject.TryGetComponent<If_Property_Result>(out var property))
        {
            bool isTrue = (bool)property.RunCode(0);
            return isTrue;
        }

        return null;
    }
}