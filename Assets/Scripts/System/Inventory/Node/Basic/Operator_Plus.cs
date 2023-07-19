using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operator_Plus : Node
{
    private DataType input0 = DataType.UNDEFINED;
    private DataType input1 = DataType.UNDEFINED;
    private DataType output = DataType.UNDEFINED;
    
    public override object RunCode(int outputIndex)
    {
        if (!CompileCountCheck())
        {
            return null;
        }
        
        /*if (GameNetWorkManager.instance.isNodeCount)
        {
            if(!GameNetWorkManager.instance.usedNodes.Contains(_nodeType))
                GameNetWorkManager.instance.usedNodes.Add(_nodeType);
        }*/
        
        float result = 0f, a = 0f, b = 0f;

        if (inputButton[0] != null)
        {
            if (input0 == DataType.FLOAT)
            {
                a = (float)inputButton[0].GetComponent<LineManager>().RunCode();
            }
            else if(input0 == DataType.INT)
            {
                a = (int)inputButton[0].GetComponent<LineManager>().RunCode();
            }
        }

        if (inputButton[1] != null)
        {
            if(input1 == DataType.FLOAT)
                b = (float)inputButton[1].GetComponent<LineManager>().RunCode();
            else if(input1 == DataType.INT)
                b = (int)inputButton[1].GetComponent<LineManager>().RunCode();
        }

        if (output == DataType.FLOAT)
            return (float)(a + b);
        
        if (output == DataType.INT)
            return (int)(a + b);
        
        return a + b;
    }

    public override void ChangeDataType()
    {
        Transform inputButtons = transform.GetChild(0);
        Transform outputButtons = transform.GetChild(1);
        
        LineManager outputButton_0 = outputButtons.GetChild(0).GetComponent<LineManager>();

        if (inputButton[0] == null)
        {
            inputButtons.GetChild(0).GetComponent<LineInputManager>().dataType = DataType.UNDEFINED;
            inputButtons.GetChild(0).GetComponent<Image>().color = Color.gray;
            input0 = DataType.UNDEFINED;
        }
        else if (inputButton[0].GetComponent<LineManager>().dataType == DataType.FLOAT)
        {
            inputButtons.GetChild(0).GetComponent<LineInputManager>().dataType = DataType.FLOAT;
            inputButtons.GetChild(0).GetComponent<Image>().color = Color.cyan;
            input0 = DataType.FLOAT;
        }
        else if (inputButton[0].GetComponent<LineManager>().dataType == DataType.INT)
        {
            inputButtons.GetChild(0).GetComponent<LineInputManager>().dataType = DataType.INT;
            inputButtons.GetChild(0).GetComponent<Image>().color = Color.green;
            input0 = DataType.INT;
        }
        
        if (inputButton[1] == null)
        {
            inputButtons.GetChild(1).GetComponent<LineInputManager>().dataType = DataType.UNDEFINED;
            inputButtons.GetChild(1).GetComponent<Image>().color = Color.gray;
            input1 = DataType.UNDEFINED;
        }
        else if (inputButton[1].GetComponent<LineManager>().dataType == DataType.FLOAT)
        {
            inputButtons.GetChild(1).GetComponent<LineInputManager>().dataType = DataType.FLOAT;
            inputButtons.GetChild(1).GetComponent<Image>().color = Color.cyan;
            input1 = DataType.FLOAT;
        }
        else if (inputButton[1].GetComponent<LineManager>().dataType == DataType.INT)
        {
            inputButtons.GetChild(1).GetComponent<LineInputManager>().dataType = DataType.INT;
            inputButtons.GetChild(1).GetComponent<Image>().color = Color.green;
            input1 = DataType.INT;
        }

        
        
        if (input0 == DataType.INT && input1 == DataType.INT)
        {
            if(outputButton_0.dataType == DataType.FLOAT)
                outputButton_0.DisconnectLineAll();
            outputButton_0.dataType = DataType.INT;
            outputButton_0.GetComponent<Image>().color = Color.green;
            output = DataType.INT;
        }
        else if (input0 == DataType.FLOAT && input1 == DataType.INT || input0 == DataType.INT && input1 == DataType.FLOAT || input0 == DataType.FLOAT && input1 == DataType.FLOAT)
        {
            if(outputButton_0.dataType == DataType.INT)
                outputButton_0.DisconnectLineAll();
            outputButton_0.dataType = DataType.FLOAT;
            outputButton_0.GetComponent<Image>().color = Color.cyan;
            output = DataType.FLOAT;
        }
        else
        {
            outputButton_0.DisconnectLineAll();
            outputButton_0.dataType = DataType.UNDEFINED;
            outputButton_0.GetComponent<Image>().color = Color.gray;
            output = DataType.UNDEFINED;
        }
    }
}