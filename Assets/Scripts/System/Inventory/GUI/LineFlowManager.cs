using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineFlowManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject nodeObject;
    public GameObject lineObject;
    public LineRenderer line;
    public DataType dataType;
    public GameObject currLineObject;
    public LineRenderer currentLine;
    
    Node node;

    bool isDrag;
    bool isConnect;
    bool isDisconnect;

    Image buttonImg;
    Color buttonColor;
    
    Sprite FillButton;
    Sprite EmptyButton;
    AudioClip dragNodeClip;
    AudioClip dropNodeClip;
    AudioClip popNodeClip;
    
    private void Awake()
    {
        line = null;
        buttonImg = GetComponent<Image>();
        buttonColor = buttonImg.color;
        FillButton = Resources.Load("Image/InventoryUI/Node/Button_Flow_1",typeof(Sprite)) as Sprite;
        EmptyButton = Resources.Load("Image/InventoryUI/Node/Button_Flow_0",typeof(Sprite)) as Sprite;
        nodeObject = transform.parent.parent.gameObject;
        node = nodeObject.GetComponent<Node>();
        dragNodeClip = Resources.Load("Sound/nodeSound/drag") as AudioClip;
        dropNodeClip = Resources.Load("Sound/nodeSound/drop") as AudioClip;
        popNodeClip = Resources.Load("Sound/nodeSound/Pop") as AudioClip;
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
            if (line != null)
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
                    if (result.gameObject.transform.parent.name == "FlowInput")
                    {
                        connectTransform = result.gameObject.transform;
                        if (connectTransform.parent.parent.gameObject.Equals(nodeObject))
                        {
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
            if (line != null)
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
                            DisconnectLine();
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
            if (line != null)
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
            if (line != null)
            {
                line.startColor = buttonColor;
                line.endColor = buttonColor;
            }
        }
    }

    public void ConnectLine(Transform connectTransform)
    {
        Node connectNode = connectTransform.parent.GetComponentInParent<Node>();
        int connectInputIndex = connectTransform.transform.GetSiblingIndex();
        int outputIndex = gameObject.transform.GetSiblingIndex();
        if (node.outputFlow[outputIndex] != null)
        {
            DisconnectLine();
        }
        node.Connect(gameObject, connectTransform.gameObject);
        currentLine.SetPosition(0, transform.position);
        currentLine.SetPosition(currentLine.positionCount - 1, connectTransform.position);
        currLineObject.GetComponent<BezierLine>().UpdateBezierLine();
        line = currentLine;
        buttonImg.sprite = FillButton;
        connectTransform.GetComponent<Image>().sprite = FillButton;
        connectTransform.GetComponent<LineFlowInputManager>().isConnect = true;
        connectTransform.GetComponent<LineFlowInputManager>().lines.Add(line);
        isConnect = true;
    }

    public void DisconnectLine()
    {
        int index = gameObject.transform.GetSiblingIndex();
        if (node.outputFlow[index] != null)
        {
            LineFlowInputManager InputFlow = node.outputFlow[index].GetComponent<LineFlowInputManager>();
            isConnect = false;
            buttonImg.sprite = EmptyButton;
            if (node.outputFlow[index].transform.parent.GetComponentInParent<Node>().inputFlow.Count == 1)
            {
                InputFlow.isConnect = false;
                node.outputFlow[index].GetComponent<Image>().sprite = EmptyButton;
            }
            node.Disconnect(gameObject, node.outputFlow[index]);
            int i = 0;
            foreach(LineRenderer inputLine in InputFlow.lines)
            {
                if (inputLine.Equals(line))
                {
                    InputFlow.lines.RemoveAt(i);
                    break;
                }
                i++;
            }
            GameObject tempLineObject = line.gameObject;
            line = null;
            Destroy(tempLineObject);
        }
    }

    public void LinePositionUpdate()
    {
        int index = gameObject.transform.GetSiblingIndex();
        if (node.outputFlow.Count != 0 && line != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(line.positionCount - 1, node.outputFlow[index].transform.position);
            line.gameObject.GetComponent<BezierLine>().UpdateBezierLine();
        }
    }
}