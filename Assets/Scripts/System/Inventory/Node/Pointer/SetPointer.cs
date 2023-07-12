using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPointer : Node
{
    [SerializeField] private bool isPuzzle;
    [SerializeField] private int puzzleNum;
    
    private void Start()
    {
        if (ItemStorage.instance != transform.root.GetComponent<ItemStorage>())
            return;
        
        if (!isPuzzle)
        {
            //PlayerSkillManager.instance.setPointer = this;
        }
        else
        {
            //PlayerSkillManager.instance.puzzleSetPointers[puzzleNum] = this;
        }
    }
    private void OnDestroy()
    {
        if (ItemStorage.instance != transform.root.GetComponent<ItemStorage>())
            return;
        
        if (!isPuzzle)
        {
            //PlayerSkillManager.instance.setPointer = null;
        }
        else
        {
            //PlayerSkillManager.instance.puzzleSetPointers[puzzleNum] = null;
        }
    }

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

        if (inputButton[0] != null)
        {
            result = (float)inputButton[0].GetComponent<LineManager>().RunCode();
        }

        return result;
    }
}