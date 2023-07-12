using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_GatlingGuIF : Node
{
    public static Node_GatlingGuIF instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        inputButton = new GameObject[INPUT_COUNT];
        for (int i = 0; i < OUTPUT_COUNT; i++)
        {
            outputButton.Add(new List<GameObject>());
        }
    }

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

        if ((float)inputButton[0].GetComponent<LineManager>().RunCode() >= 12f &&
        (float)inputButton[0].GetComponent<LineManager>().RunCode() <= 14f)
        {
            return true;
        }

        return null;
    }
}
