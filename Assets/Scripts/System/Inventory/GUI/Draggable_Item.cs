using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable_Item : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject nodeObject;
    protected  Transform tempNodeObject;
    public GraphicRaycaster gr;
    protected Camera cam;
    protected Transform canvas;
    protected Transform property;

    protected bool isDrag;

    protected AudioClip startClickClip;
    protected AudioClip startEndClip;

    public Image image;
    public TMP_Text title;
    public TMP_Text info;
    public TMP_Text Count;

    protected Sprite tempImage;
    protected string tempTitle;
    protected string tempInfo;
    protected int tempCount;

    void Awake()
    {
        startClickClip = Resources.Load("Sound/nodeConnectSound/drag_[cut_0sec]") as AudioClip;
        startEndClip = Resources.Load("Sound/nodeConnectSound/dragEnd_[cut_0sec]") as AudioClip;
        isDrag = false;
    }

    protected void Start()
    {
        canvas = InventoryField.instance.transform;
        cam = GameObject.Find("InventoryCamera").GetComponent<Camera>();
        property = GameObject.Find("InventoryUI").transform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.instance.SFXPlayOneShot("startClick", startClickClip);
        if (nodeObject != null)
        {
            isDrag = true;
            tempNodeObject = Instantiate(nodeObject, canvas).transform;
            tempNodeObject.SetParent(property);
            tempNodeObject.SetAsLastSibling();
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -cam.transform.position.z - 200f));
            tempNodeObject.position = point;

            if (ItemListUI.instance.isInfinityMode)
            {
                if(!ItemManager.instance.allowedInventoryItems.Contains((NodeType)Enum.Parse(typeof(NodeType), nodeObject.GetComponent<Node>().NODE_TYPE)))
                    return;
            }

            tempCount = int.Parse(Count.text);
            if (tempCount == 1)
            {
                image.enabled = false;
                tempTitle = title.text;
                tempInfo = info.text;
                title.text = "";
                info.text = "";
                Count.text = "0";
            }
            else if(tempCount == 2)
            {
                Count.GetComponentInParent<Image>().enabled = false;
                Count.enabled = false;
                Count.text = "1";
            }
            else
            {
                Count.text = $"{(tempCount - 1)}";
            }
            tempCount--;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag == true)
        {
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -cam.transform.position.z -200f));
            tempNodeObject.position = point;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.instance.SFXPlayOneShot("endClick", startEndClip);
        if (isDrag == true)
        {
            isDrag = false;
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
                        if (ItemListUI.instance.isInfinityMode)
                        {
                            if (!ItemManager.instance.allowedInventoryItems.Contains(
                                    (NodeType)Enum.Parse(typeof(NodeType), nodeObject.GetComponent<Node>().NODE_TYPE)))
                            {
                                Destroy(tempNodeObject.gameObject);
                                return;
                            }
                        }

                        if (tempCount == 0)
                        {
                            Destroy(tempNodeObject.gameObject);
                            title.text = tempTitle;
                            info.text = tempInfo;
                            Count.text = "1";
                            image.enabled = true;
                        }
                        else
                        {
                            Destroy(tempNodeObject.gameObject);
                            Count.enabled = true;
                            Count.text = $"{tempCount + 1}";
                            Count.GetComponentInParent<Image>().enabled = true;
                        }
                        tempCount++;
                        return;
                    }
                }
            }
            tempNodeObject.SetParent(canvas);
            
            if (ItemListUI.instance.isInfinityMode)
            {
                if(!ItemManager.instance.allowedInventoryItems.Contains((NodeType)Enum.Parse(typeof(NodeType), nodeObject.GetComponent<Node>().NODE_TYPE)))
                    return;
            }
            
            ItemManager.instance.RemoveItem((NodeType)Enum.Parse(typeof(NodeType), nodeObject.GetComponent<Node>().NODE_TYPE), ItemListUI.instance.isPresetMode);
            if (tempCount == 0)
            {
                nodeObject = null;
            }
        }
    }

    public void ResetItemSlot()
    {
        nodeObject = null;
        image.enabled = false;
        tempTitle = title.text;
        tempInfo = info.text;
        title.text = "";
        info.text = "";
        Count.GetComponentInParent<Image>().enabled = false;
        Count.enabled = false;
        Count.text = "0";
    }
}