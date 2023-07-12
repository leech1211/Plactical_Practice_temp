using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable_Parts : Draggable
{
    private GraphicRaycaster gr2;

    private void Start()
    {
        gr2 = InventoryField.instance.GetComponent<GraphicRaycaster>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.instance.SFXPlayOneShot("startClick", startClickClip);

        isClick = true;
        transform.SetParent(propertyTr);
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -cam.transform.position.z));
        clickPosition = point - transform.position;
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
                    }
                }
            }
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0) && isClick)
        {
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -cam.transform.position.z));
            transform.position = point - clickPosition;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
        SoundManager.instance.SFXPlayOneShot("endClick", startEndClip);

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
                    mouseControl.DeleteNode();
                    return;
                }
            }
        }
        
        ped.position = Input.mousePosition;
        gr2.Raycast(ped,results);
        if (results.Count != 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("PartsSlot"))
                {
                    Transform slot = result.gameObject.transform;
                    if (slot.childCount >= 1)
                    {
                        Transform prevParts = slot.GetChild(0);
                        prevParts.position = new Vector3(prevParts.position.x + 50f, prevParts.position.y - 50f,
                            prevParts.position.z);
                        prevParts.SetParent(InventoryField.instance.transform);
                    }
                    transform.SetParent(slot);
                    transform.localPosition = Vector3.zero;
                }
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
