using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomIn : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Camera.main.orthographicSize >= 300)
        {
            Camera.main.orthographicSize -= 100;
        }
        else
        {
            Camera.main.orthographicSize = 200;
        }
    }
}
