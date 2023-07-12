using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable_Toolbar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject nodeObject;
    private Transform tempNodeObject;
    Transform canvas;
    Vector3 clickPosition;
    RectTransform rectTr;

    AudioClip startClickClip;
    AudioClip startEndClip;
    
    void Awake()
    {
        startClickClip = Resources.Load("Sound/nodeConnectSound/drag_[cut_0sec]") as AudioClip;
        startEndClip = Resources.Load("Sound/nodeConnectSound/dragEnd_[cut_0sec]") as AudioClip;
        rectTr = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SoundManager.instance.SFXPlayOneShot("startClick", startClickClip);
        tempNodeObject = Instantiate(nodeObject, canvas).transform;
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
        clickPosition = point - transform.position;
        tempNodeObject.SetParent(canvas);
        tempNodeObject.position = clickPosition;
        tempNodeObject.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.z));
        tempNodeObject.position = point - clickPosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        SoundManager.instance.SFXPlayOneShot("endClick", startEndClip);
    }
}
