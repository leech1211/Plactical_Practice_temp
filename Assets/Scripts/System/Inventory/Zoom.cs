using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private InventoryCameraManager iCamManager;
    [SerializeField] private bool isZoomOut;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isZoomOut)
        {
            iCamManager.ZoomOut();
        }
        else
        {
            iCamManager.ZoomIn();
        }
    }
}
