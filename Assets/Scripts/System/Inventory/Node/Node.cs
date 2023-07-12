using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header ("Node Setting")]
    public string NODE_TYPE;
    protected NodeType _nodeType;
    [HideInInspector] public GameObject[] inputButton;
    [HideInInspector] public List<List<GameObject>> outputButton = new List<List<GameObject>>();
    [HideInInspector] public List<GameObject> inputFlow;
    [HideInInspector] public List<GameObject> outputFlow;
    [HideInInspector] public string variableName;

    public int INPUT_COUNT;
    public int OUTPUT_COUNT;
    public int INPUT_FLOW_COUNT;
    public int OUTPUT_FLOW_COUNT;
    
    public bool isStatic;

    [Header ("Node Info")]
    public Sprite Icon;
    public string PrefebPath;
    public string Title;
    [TextArea] public string Information;

    private void Awake()
    {
        if(INPUT_COUNT > 0)
            inputButton = new GameObject[INPUT_COUNT];
        for (int i = 0; i < OUTPUT_COUNT; i++)
        {
            outputButton.Add(new List<GameObject>());
        }
        if(OUTPUT_FLOW_COUNT > 0)
            outputFlow = Enumerable.Repeat(default(GameObject), OUTPUT_FLOW_COUNT).ToList();
        
        _nodeType = (NodeType)Enum.Parse(typeof(NodeType), NODE_TYPE);
    }

    //Data 기반호출
    public virtual object RunCode(int outputIndex)
    {
        return null;
    }

    //Flow 기반호출
    public async virtual Task<object> RunCode(NodeFlag parent)
    {
        return null;
    }

    public bool CompileCountCheck()
    {
        if (InventoryField.instance.compileCounter > 300)
        {
            return false;
        }

        InventoryField.instance.compileCounter++;
        return true;
    }

    public void SetInput(GameObject outputButton, int index)
    {
        inputButton[index] = outputButton;
    }

    public void Connect(GameObject startButton, GameObject endButton)
    {
        // 여기!!
        if (startButton.transform.parent.name == "FlowOutput")
        {
            int startIndex = startButton.transform.GetSiblingIndex();
            outputFlow[startIndex] = endButton;
            endButton.transform.parent.GetComponentInParent<Node>().inputFlow.Add(startButton);
        }
        // 끝
        else
        {
            int startIndex = startButton.transform.GetSiblingIndex();
            int endIndex = endButton.transform.GetSiblingIndex();
            outputButton[startIndex].Add(endButton);
            endButton.transform.parent.GetComponentInParent<Node>().SetInput(startButton, endIndex);
        }
    }

    public int Disconnect(GameObject startButton, GameObject endButton)
    {
        int startIndex = startButton.transform.GetSiblingIndex();
        if (startButton.transform.parent.name == "FlowOutput")
        {
            if (endButton != null)
            {
                endButton.transform.parent.GetComponentInParent<Node>().inputFlow.Remove(startButton);
                outputFlow[startIndex] = null;
                return 0;
            }
        }
        else
        {
            int endIndex = endButton.transform.GetSiblingIndex();
            endButton.transform.parent.GetComponentInParent<Node>().inputButton[endIndex] = null;
            for (int i = 0; i < outputButton[startIndex].Count; i++)
            {
                if (outputButton[startIndex][i].Equals(endButton))
                {
                    outputButton[startIndex].RemoveAt(i);
                    return i;
                }
            }
        }
        return -1;
    }

    public void DisconnectAll(GameObject startButton)
    {
        int startIndex = startButton.transform.GetSiblingIndex();
        int connectCount = outputButton[startIndex].Count;
        for (int i = 0; i < connectCount; i++)
        {
            GameObject endButton = outputButton[startIndex][0];
            if (endButton != null)
            {
                Disconnect(startButton, endButton);
            }
        }
    }

    public void ConstantNode()
    {
        isStatic = true;
        GetComponent<Draggable>().isActive = false;

        for (int i = 0; i < INPUT_COUNT; i++)
        {
            if (inputButton[i] == null)
                continue;
            inputButton[i].transform.parent.parent.GetComponent<Node>().ConstantNode();
        }
    }

    public virtual void ChangeDataType()
    {
        
    }
}