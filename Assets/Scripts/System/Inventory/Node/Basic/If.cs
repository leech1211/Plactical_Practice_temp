using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class If : Node
{
    public override object RunCode(int outputIndex)
    {
        if (!CompileCountCheck())
        {
            return null;
        }

        if (GameNetWorkManager.instance.isNodeCount)
        {
            if (!GameNetWorkManager.instance.usedNodes.Contains(_nodeType))
                GameNetWorkManager.instance.usedNodes.Add(_nodeType);
        }

        bool result = (bool)inputButton[0].GetComponent<LineManager>().RunCode();

        if (result)
        {
            return outputFlow[0].transform.parent.parent.GetComponent<Node>().RunCode(0);
        }
        else
        {
            return outputFlow[1].transform.parent.parent.GetComponent<Node>().RunCode(0);
        }

        return null;
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

        bool result = (bool)inputButton[0].GetComponent<LineManager>().RunCode();

        if (result)
        {
            await outputFlow[0].transform.parent.parent.GetComponent<Node>().RunCode(parent);
        }
        else
        {
            await outputFlow[1].transform.parent.parent.GetComponent<Node>().RunCode(parent);
        }

        return null;
    }
}