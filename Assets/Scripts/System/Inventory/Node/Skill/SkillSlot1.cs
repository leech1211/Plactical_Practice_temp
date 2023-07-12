using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot1 : Node
{
    public override object RunCode(int outputIndex)
    {
        InventoryField.instance.compileCounter = 0;
        if (!CompileCountCheck())
        {
            return null;
        }
        
        if (outputFlow[0] != null)
        {
            return outputFlow[0].GetComponent<LineManager>().RunCode();
        }

        return null;
    }
}