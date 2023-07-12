using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineInputManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject nodeObject;
    public GameObject lineObject;
    public DataType dataType;
    public bool isConnect;
    public LineRenderer line;
    public bool SingleConnectMode;
    public List<DataType> multipleDataType;

    bool isDrag;
    bool isDisconnect;

    GameObject currLineObject;
    LineRenderer currentLine;

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
        buttonImg = GetComponent<Image>();
        buttonColor = buttonImg.color;
        isConnect = false;
        isDrag = false;
        isDisconnect = false;
        line = null;
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
            currentLine.SetPosition(currentLine.positionCount - 1, transform.position);
            currentLine.SetPosition(0, transform.position);
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
            point = new Vector3(point.x, point.y, transform.position.z);
            currentLine.SetPosition(0, point);
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
                    if (result.gameObject.transform.parent.name == "Output")
                    {
                        connectTransform = result.gameObject.transform;
                        if (connectTransform.parent.parent.gameObject.Equals(nodeObject))
                        {
                            break;
                        }
                        else if (result.gameObject.GetComponent<LineManager>().dataType != dataType)
                        {
                            if(multipleDataType == null)
                                break;
                            if(multipleDataType.Contains(result.gameObject.GetComponent<LineManager>().dataType) == false)
                                break;
                        }
                        SoundManager.instance.SFXPlayOneShot("dropNode", dropNodeClip);
                        LineManager outputLineManager = connectTransform.GetComponent<LineManager>();
                        outputLineManager.currLineObject = currLineObject;
                        outputLineManager.currentLine = currentLine;
                        currLineObject.transform.SetParent(connectTransform);
                        outputLineManager.ConnectLine(transform);
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
                            int inputIndex = transform.GetSiblingIndex();
                            GameObject startButton = nodeObject.GetComponent<Node>().inputButton[inputIndex];
                            if (startButton != null)
                            {
                                startButton.GetComponent<LineManager>().DisconnectLine(gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isConnect == false && isDrag == false)
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
        if (isConnect == false && isDrag == false)
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
}