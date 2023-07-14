using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite NormalImg;
    public Sprite SelectImg;
    public bool isActive = true;
    protected GraphicRaycaster gr;
    protected Camera cam;
    protected InventoryManager mouseControl;
    protected Vector3 clickPosition;
    protected Transform propertyTr;

    protected AudioClip startClickClip;
    protected AudioClip startEndClip;

    protected bool isClick;

    void OnEnable()
    {
        StartCoroutine(WaitInitialize());
    }

    private IEnumerator WaitInitialize()
    {
        while (InventoryManager.instance == null)
            yield return null;
        Initialize();
    }

    public void Initialize()
    {
        startClickClip = Resources.Load("Sound/nodeConnectSound/drag_[cut_0sec]") as AudioClip;         //사운드파일 가져오기
        startEndClip = Resources.Load("Sound/nodeConnectSound/dragEnd_[cut_0sec]") as AudioClip;        //사운드 파일 가져오기
       
        cam = GameObject.Find("InventoryCamera").GetComponent<Camera>();
        GameObject property = GameObject.Find("InventoryUI");
        if (property != null)
        {
            mouseControl = property.GetComponent<InventoryManager>();
            propertyTr = property.transform;
            gr = property.GetComponent<GraphicRaycaster>();
        }

        isClick = false;
    }
    
    //대충 클릭했을 때 위치를 변경?
    public virtual void OnPointerDown(PointerEventData eventData)                           //클릭 내려갈때 호출
    {
        //Debug.Log("start사운드의 길이 : " + startClickClip.length);
        SoundManager.instance.SFXPlayOneShot("startClick", startClickClip);         //startClickClip을 한번 재생
        if (Input.GetKey(KeyCode.Mouse0) && isActive)                                      //좌클릭 했을 시
        {
            isClick = true;
            transform.SetParent(propertyTr);
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -cam.transform.position.z));
            clickPosition = point - transform.position;
            UpdateLines();
            transform.SetAsLastSibling();
            if (mouseControl.selectedObject.Count > 1)
            {
                if (mouseControl.selectedObject.Contains(gameObject))
                {
                    foreach (GameObject tempNode in mouseControl.selectedObject)
                    {
                        if (!gameObject.Equals(tempNode))
                        {
                            tempNode.transform.SetParent(gameObject.transform);
                            tempNode.GetComponent<Draggable>().UpdateLines();
                        }
                    }
                }
            }
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0) && isActive && isClick)
        {
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -cam.transform.position.z));
            transform.position = point - clickPosition;
            UpdateLines();
            if (mouseControl.selectedObject.Count > 1)
            {
                if (mouseControl.selectedObject.Contains(gameObject))
                {
                    foreach (GameObject tempNode in mouseControl.selectedObject)
                    {
                        if (!gameObject.Equals(tempNode))
                        {
                            tempNode.GetComponent<Draggable>().UpdateLines();
                        }
                    }
                }
            }
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
        //Debug.Log("end사운드의 길이 : " + startClickClip.length);
        SoundManager.instance.SFXPlayOneShot("endClick", startEndClip);
        if (isActive)
        {
            transform.SetParent(InventoryField.instance.transform);
            if (mouseControl.selectedObject.Count > 1)
            {
                if (mouseControl.selectedObject.Contains(gameObject))
                {
                    foreach (GameObject tempNode in mouseControl.selectedObject)
                    {
                        if (!gameObject.Equals(tempNode))
                        {
                            tempNode.transform.SetParent(gameObject.transform.parent);
                        }
                    }
                }
            }
        }

        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("InventoryUI"))
                {
                    mouseControl.DeleteNode(gameObject);
                }
            }
        }
    }

    public void UpdateLines()
    {
        Node node = gameObject.GetComponent<Node>();
        foreach (GameObject button in node.inputButton)
        {
            if (button != null)
            {
                button.GetComponent<LineManager>().LinePositionUpdate();
            }
        }

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            transform.GetChild(1).GetChild(i).GetComponent<LineManager>().LinePositionUpdate();
        }

        foreach (GameObject flowButton in node.inputFlow)
        {
            if (flowButton != null)
            {
                flowButton.GetComponent<LineFlowManager>().LinePositionUpdate();
            }
        }

        if (node.outputFlow.Count != 0 && node.OUTPUT_FLOW_COUNT != 0)
        {
            LineFlowManager[] FlowLines = transform.GetChild(3).GetComponentsInChildren<LineFlowManager>();
            foreach (LineFlowManager flowLine in FlowLines)
            {
                flowLine.LinePositionUpdate();
            }
        }
    }

    public void SetNormal()
    {
        GetComponent<Image>().sprite = NormalImg;
    }
    
    public void SetSelect()
    {
        GetComponent<Image>().sprite = SelectImg;
    }
}
