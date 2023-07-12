using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class If_Property_Result : Node
{
    public override object RunCode(int outputIndex)
    {
        if (!CompileCountCheck())
        {
            return null;
        }

        if (inputButton[0] != null)
        {
            return inputButton[0].GetComponent<LineManager>().RunCode();
        }

        return null;
    }
}