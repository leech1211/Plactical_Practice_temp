﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2_Result : Node
{
    public override object RunCode(int outputIndex)
    {
        InventoryField.instance.compileCounter = 0;
        
        if (!CompileCountCheck())
        {
            return null;
        }

        if (inputButton[0] == null)
        {
            return false;
        }

        if ((float)inputButton[0].GetComponent<LineManager>().RunCode() < -13f)
        {
            ConstantNode();
            return true;
        }

        return null;
    }
}