using System.Collections.Generic;
using UnityEngine;

public class GetPointer : Node
{
    [SerializeField] private bool isPuzzle;
    [SerializeField] private int puzzleNum;
    
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

        float result = 0f;

        Node setPointer = null;
        if (!isPuzzle)
        {
            //setPointer = PlayerSkillManager.instance.setPointer;
        }
        else
        {
            //setPointer = PlayerSkillManager.instance.puzzleSetPointers[puzzleNum];
        }

        if (setPointer != null)
        {
            result = (float)setPointer.RunCode(0);
        }
        Debug.Log(result);
        return result;
    }
}