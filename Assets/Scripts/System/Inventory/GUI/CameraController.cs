using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 200.0f;
    public Camera cam;
    public GraphicRaycaster gr;
    public Scrollbar inScroll;
    Vector3 startPosition;
    Rigidbody2D rig;
    float moveSpeed;

    void Start()
    {
        cam = GetComponent<Camera>();
        rig = GetComponent<Rigidbody2D>();
        moveSpeed = 10f;
    }

    void Update()
    {
        float InventoryScroll = Input.GetAxis("Mouse ScrollWheel");
        float scroll = InventoryScroll * cameraSpeed;

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
            transform.position = transform.position + (startPosition - point);
            startPosition = point;
        }

        if (Mathf.Abs(scroll) > 0.1f)
        {
            if (CheckInventory() == true)
            {
                inScroll.value -= InventoryScroll;
                if (inScroll.value < 0f)
                {
                    inScroll.value = 0f;
                }
                else if (inScroll.value > 1f)
                {
                    inScroll.value = 1f;
                }
            }
            // Max Zoom in
            else if (cam.orthographicSize <= 200.0f && scroll > 0)
            {
                cam.orthographicSize = 200.0f;

            }
            // Max Zoom out
            else if (cam.orthographicSize >= 600.0f && scroll < 0)
            {
                cam.orthographicSize = 600.0f;
            }
            else
            {
                cam.orthographicSize -= scroll;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (dir.magnitude < 0.1f)
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed += 10f;
        }

        rig.velocity = dir * moveSpeed;
    }

    bool CheckInventory()
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if (results.Count != 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "ToolBar")
                {
                    return true;
                }
            }
        }

        return false;
    }
}
