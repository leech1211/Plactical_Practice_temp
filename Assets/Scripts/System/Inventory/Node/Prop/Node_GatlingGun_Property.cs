using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_GatlingGun_Property : Node
{
    public static Node_GatlingGun_Property instance;

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
        if (!CompileCountCheck())
        {
            return null;
        }

        List<float> result = new List<float>{0, 0};

        if (inputButton[0] != null)
        {
            result[0] = (float)inputButton[0].GetComponent<LineManager>().RunCode();
        }

        // if (inputButton[1] != null)
        // {
        //     result[1] = NodeType.NULL;
        // }
        return result;
    }
}