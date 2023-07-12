using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3_Result : Node
{
    public override object RunCode(int outputIndex)
    {
        InventoryField.instance.compileCounter = 0;
        if (!CompileCountCheck())
        {
            return null;
        }

        if (inputButton[0] != null)
        {
            float result = (float)inputButton[0].GetComponent<LineManager>().RunCode();
            if (result == 6f)
                return true;
            return false;
        }

        return null;
    }
}