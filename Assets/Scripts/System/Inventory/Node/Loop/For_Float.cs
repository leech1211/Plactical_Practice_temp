using System.Collections.Generic;
using UnityEngine;

public class For_Float : Node
{
    public Transform partsSlotTransform;
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
        
        GameObject parts = partsSlotTransform.GetChild(0).gameObject;
        float result = 0f, a = 0f, b = 0f;

        if (inputButton[0] != null)
        {
            a = (float)inputButton[0].GetComponent<LineManager>().RunCode();
            result = a;
        }

        if (inputButton[1] != null)
        {
            b = (float)inputButton[1].GetComponent<LineManager>().RunCode();
        }

        if (partsSlotTransform.childCount == 0)
        {
            result = a;
        }
        else if (parts.TryGetComponent<Plus>(out var plus))
        {
            for (int i = 0; i < b; i++)
            {
                result += a;
                if (!CompileCountCheck())
                {
                    return null;
                }
            }
        }
        else if (parts.TryGetComponent<Minus>(out var minus))
        {
            for (int i = 0; i < b; i++)
            {
                result -= a;
                if (!CompileCountCheck())
                {
                    return null;
                }
            }
        }
        else if (parts.TryGetComponent<Multiply>(out var multiply))
        {
            for (int i = 0; i < b; i++)
            {
                result *= a;
                if (!CompileCountCheck())
                {
                    return null;
                }
            }
        }
        else if (parts.TryGetComponent<Division>(out var division))
        {
            for (int i = 0; i < b; i++)
            {
                result /= a;
                if (!CompileCountCheck())
                {
                    return null;
                }
            }
        }

        Debug.Log(result);
        return result;
    }
}