using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineManager : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject nodeObject;
    public GameObject lineObject;
    public List<LineRenderer> lines;
    public DataType dataType;
    public GameObject currLineObject;
    public LineRenderer currentLine;
    public bool SingleConnectMode;

    bool isDrag;
    bool isConnect;
    bool isDisconnect;
    
    Node node;

    Image buttonImg;
    Color buttonColor;
    
    Sprite FillButton;
    Sprite EmptyButton;
    AudioClip dragNodeClip;
    AudioClip dropNodeClip;
    AudioClip popNodeClip;

    private void Awake()
    {
        FillButton = Resources.Load("Image/InventoryUI/Node/Button_1",typeof(Sprite)) as Sprite;
        EmptyButton = Resources.Load("Image/InventoryUI/Node/Button_0",typeof(Sprite)) as Sprite;
        dragNodeClip = Resources.Load("Sound/nodeSound/drag") as AudioClip;
        dropNodeClip = Resources.Load("Sound/nodeSound/drop") as AudioClip;
        popNodeClip = Resources.Load("Sound/nodeSound/Pop") as AudioClip;
        nodeObject = transform.parent.parent.gameObject;
        node = nodeObject.GetComponent<Node>();
        buttonImg = GetComponent<Image>();
        buttonColor = buttonImg.color;
        isDrag = false;
        isConnect = false;
        isDisconnect = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) == false)
        {
            isDrag = true;
            buttonImg.sprite = FillButton;
            SoundManager.instance.SFXPlayOneShot("dragNode", dragNodeClip);
            Color buttonColor = buttonImg.color;
            currLineObject = Instantiate(lineObject, transform);
            currentLine = currLineObject.GetComponent<LineRenderer>();
            currentLine.startColor = buttonColor;
            currentLine.endColor = buttonColor;
            currentLine.SetPosition(0, transform.position);
            currentLine.SetPosition(currentLine.positionCount - 1, transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && Input.GetKey(KeyCode.Mouse0) == false)
        {
            SoundManager.instance.SFXPlayOneShot("dragNode", dragNodeClip);
            buttonImg.color = Color.red;
            foreach (LineRenderer line in lines)
            {
                line.startColor = Color.red;
                line.endColor = Color.red;
            }
            isDisconnect = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag == true)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
            point = new Vector3(point.x, point.y, transform.position.z);
            currentLine.SetPosition(currentLine.positionCount - 1, point);
            currLineObject.GetComponent<BezierLine>().UpdateBezierLine();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        InventoryField.instance.GetComponent<GraphicRaycaster>().Raycast(ped, results);
        if (isDrag == true)
        {
            isDrag = false;
            Transform connectTransform;
            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.transform.parent.name == "Input")
                    {
                        connectTransform = result.gameObject.transform;
                        if (connectTransform.parent.parent.gameObject.Equals(nodeObject))
                        {
                            break;
                        }
                        else if (result.gameObject.GetComponent<LineInputManager>().dataType != dataType)
                        {
                            var dataTypes = result.gameObject.GetComponent<LineInputManager>().multipleDataType;
                            if(dataTypes == null)
                                break;
                            if(dataTypes.Contains(dataType) == false)
                                break;
                        }
                        SoundManager.instance.SFXPlayOneShot("dropNode", dropNodeClip);
                        ConnectLine(connectTransform);
                        return;
                    }
                }
                
            }
            SoundManager.instance.SFXPlayOneShot("popNode", popNodeClip);
            if (isConnect == false)
            {
                buttonImg.sprite = EmptyButton;
            }
            Destroy(currLineObject);
        }

        if (isDisconnect == true)
        {
            isDisconnect = false;
            buttonImg.color = buttonColor;
            foreach (LineRenderer line in lines)
            {
                line.startColor = buttonColor;
                line.endColor = buttonColor;
            }
            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.Equals(gameObject))
                    {
                        SoundManager.instance.SFXPlayOneShot("popNode", popNodeClip);
                        if (isConnect == true)
                        {
                            DisconnectLineAll();
                        }
                    }
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDrag == false && isConnect == false)
        {
            buttonImg.sprite = FillButton;
        }

        if (isDisconnect == true)
        {
            buttonImg.color = Color.red;
            foreach (LineRenderer line in lines)
            {
                line.startColor = Color.red;
                line.endColor = Color.red;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDrag == false && isConnect == false)
        {
            buttonImg.sprite = EmptyButton;
        }
        
        if (isDisconnect == true)
        {
            buttonImg.color = buttonColor;
            foreach (LineRenderer line in lines)
            {
                line.startColor = buttonColor;
                line.endColor = buttonColor;
            }
        }
    }

    public void ConnectLine(Transform connectTransform)
    {
        if (SingleConnectMode)
        {
            DisconnectLineAll();
        }
        
        Node connectNode = connectTransform.parent.GetComponentInParent<Node>();
        int connectInputIndex = connectTransform.transform.GetSiblingIndex();
        //Debug.Log("connectInputIndex : " + connectInputIndex);
        //Debug.Log("connectNode.inputButton.Length : " + connectNode.inputButton.Length);
        if (connectNode.inputButton[connectInputIndex] != null)
        {
            connectNode.inputButton[connectInputIndex].GetComponent<LineManager>().DisconnectLine(connectTransform.gameObject);
        }
        node.Connect(gameObject, connectTransform.gameObject);
        currentLine.SetPosition(0, transform.position);
        currentLine.SetPosition(currentLine.positionCount - 1, connectTransform.position);
        currLineObject.GetComponent<BezierLine>().UpdateBezierLine();
        lines.Add(currentLine);
        buttonImg.sprite = FillButton;
        connectTransform.GetComponent<Image>().sprite = FillButton;
        connectTransform.GetComponent<LineInputManager>().isConnect = true;
        connectTransform.GetComponent<LineInputManager>().line = currentLine;
        isConnect = true;
        connectNode.ChangeDataType();
    }

    public void DisconnectLine(GameObject endButton)
    {
        if (lines.Count == 1)
        {
            buttonImg.sprite = EmptyButton;
            isConnect = false;
        }
        endButton.GetComponent<Image>().sprite = EmptyButton;
        endButton.GetComponent<LineInputManager>().isConnect = false;
        int lineIndex = node.Disconnect(gameObject, endButton);
        if (lineIndex < lines.Count)
        {
            GameObject tempLineObject = lines[lineIndex].gameObject;
            lines.RemoveAt(lineIndex);
            Destroy(tempLineObject);
        }
        endButton.GetComponent<LineInputManager>().nodeObject.GetComponent<Node>().ChangeDataType();
    }

    public void DisconnectLineAll()
    {
        List<Node> connectedNodes = new List<Node>();
        
        foreach (LineRenderer line in lines)
        {
            Destroy(line.gameObject);
        }
        lines.Clear();
        List<GameObject> endButtons = node.outputButton[gameObject.transform.GetSiblingIndex()];
        foreach (GameObject endButton in endButtons)
        {
            endButton.GetComponent<Image>().sprite = EmptyButton;
            endButton.GetComponent<LineInputManager>().isConnect = false;
            connectedNodes.Add(endButton.GetComponent<LineInputManager>().nodeObject.GetComponent<Node>());
        }
        buttonImg.sprite = EmptyButton;
        isConnect = false;
        node.DisconnectAll(gameObject);
        foreach(var node in connectedNodes)
        {
            node.ChangeDataType();
        }
    }

    public void LinePositionUpdate()
    {
        int index = gameObject.transform.GetSiblingIndex();
        if (node.outputButton[index] != null)
        {
            for (int i = 0; i < node.outputButton[index].Count; i++)
            {
                lines[i].SetPosition(0, transform.position);
                lines[i].SetPosition(lines[i].positionCount - 1, node.outputButton[index][i].transform.position);
                lines[i].gameObject.GetComponent<BezierLine>().UpdateBezierLine();
            }
        }
    }

    public object RunCode()
    {
        return nodeObject.GetComponent<Node>().RunCode(transform.GetSiblingIndex());
    }
}