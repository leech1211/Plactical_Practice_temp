using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOM_PROPERTY : Node
{
    public static BOOM_PROPERTY instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        inputButton = new GameObject[INPUT_COUNT];

    }
    public override object RunCode(int outputIndex)     //오버라이드
    {
        if (!CompileCountCheck())                       //유효성 검사
        {
            return null;
        }
        List<float> result = new List<float>(){0,0,0};      //각각 공격력, 체력, 폭발시간
        //Debug.Log("inputButton.Length : " + inputButton.Length);
        if (inputButton[0] != null)
        {
            result[0] = (float)inputButton[0].GetComponent<LineManager>().RunCode();
        }
        if (inputButton[1] != null)
        {
            result[1] = (float)inputButton[1].GetComponent<LineManager>().RunCode();
        }
        
        if (inputButton[2] != null)
        {
            result[2] = (float)inputButton[2].GetComponent<LineManager>().RunCode();
        }
        return result;
    }
}
