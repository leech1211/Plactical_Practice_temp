public class NodeFlag
{
    public NodeFlag parent = null;
    public bool Stop
    {
        get
        {
            NodeFlag tempFlag = this;
            while (tempFlag != null)
            {
                if(tempFlag.isBreak)
                    return true;
                tempFlag = tempFlag.parent;
            }
            return false;
        }
    }
    public bool isBreak;
}
