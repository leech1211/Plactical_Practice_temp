using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToolBarHideShow : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RectTransform toolBar;
    [SerializeField] private Vector3 openedPos;
    [SerializeField] private Vector3 closedPos;
    public bool isOpened;

    private void Start()
    {
        isOpened = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpened == false)
            {
                StopAllCoroutines();
                StartCoroutine(Open());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Close());
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOpened == false)
        {
            StopAllCoroutines();
            StartCoroutine(Open());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
    }
    
    IEnumerator Open()
    {
        isOpened = true;
        float time = 0f;
        float timeSlerp;
        Vector3 currPos = toolBar.anchoredPosition;
        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            timeSlerp = (-Mathf.Cos(time * Mathf.PI) + 1) / 2;
            toolBar.anchoredPosition = Vector3.Lerp(currPos, openedPos, timeSlerp);
            yield return null;
        }

        toolBar.anchoredPosition = openedPos;
    }

    IEnumerator Close()
    {
        isOpened = false;
        float time = 0f;
        float timeSlerp;
        Vector3 currPos = toolBar.anchoredPosition; 
        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            timeSlerp = (-Mathf.Cos(time * Mathf.PI) + 1) / 2;
            toolBar.anchoredPosition = Vector3.Lerp(currPos, closedPos, timeSlerp);
            yield return null;
        }

        toolBar.anchoredPosition = closedPos;
    }
}
