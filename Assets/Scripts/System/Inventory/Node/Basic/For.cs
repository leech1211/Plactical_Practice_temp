using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class For : Node
{
    int index = 0;

    public override object RunCode(int outputIndex)
    {
        return index;
    }

    public async override Task<object> RunCode(NodeFlag parent)
    {
        if (!CompileCountCheck() || parent.Stop)
            return null;

        if (GameNetWorkManager.instance.isNodeCount)
        {
            if (!GameNetWorkManager.instance.usedNodes.Contains(_nodeType))
                GameNetWorkManager.instance.usedNodes.Add(_nodeType);
        }
        
        //OpenGate(false);

        int count = (int)inputButton[0].GetComponent<LineManager>().RunCode();
        NodeFlag flag = new NodeFlag();
        flag.parent = parent;
        
        for (int i = 0; i < count; i++)
        {
            index = i;
            if (outputFlow[1] != null)
            {
                await outputFlow[1].transform.parent.parent.GetComponent<Node>().RunCode(flag);
            }
            
            if(flag.isBreak)
                break;
        }

        index = 0;
        if (outputFlow[0] != null)
        {
            await outputFlow[0].transform.parent.parent.GetComponent<Node>().RunCode(parent);
        }

        return null;
    }
}