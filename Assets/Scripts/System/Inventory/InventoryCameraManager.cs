using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryCameraManager : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 200.0f;
    [SerializeField] private GraphicRaycaster gr;

    private Camera cam;
    private Vector3 startPosition;
    private Rigidbody2D rig;
    private float moveSpeed;
    private Vector3 BasePosition;
    private float baseOrthographicSize;

    void Start()
    {
        cam = GetComponent<Camera>();
        rig = GetComponent<Rigidbody2D>();
        moveSpeed = 10f;
        baseOrthographicSize = cam.orthographicSize;
        BasePosition = transform.position;
    }

    // void Update()
    // {
    //     float InventoryScroll = Input.GetAxis("Mouse ScrollWheel");
    //     float scroll = InventoryScroll * cameraSpeed;
    //
    //     if (Mathf.Abs(scroll) > 0.1f)
    //     {
    //         // Max Zoom in
    //         if (cam.orthographicSize <= 200.0f && scroll > 0)
    //         {
    //             cam.orthographicSize = 200.0f;
    //
    //         }
    //         // Max Zoom out
    //         else if (cam.orthographicSize >= 600.0f && scroll < 0)
    //         {
    //             cam.orthographicSize = 600.0f;
    //         }
    //         else
    //         {
    //             cam.orthographicSize -= scroll;
    //         }
    //     }
    // }

    public void SetMoveCamera()
    {
        startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10) * cam.orthographicSize / baseOrthographicSize;
    }
    
    public void UpdateMoveCamera()
    {
        Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10) * cam.orthographicSize / baseOrthographicSize;
        transform.position = transform.position + (startPosition - point);
        startPosition = point;
    }

    // void FixedUpdate()
    // {
    //     Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    //     if (dir.magnitude < 0.1f)
    //     {
    //         moveSpeed = 10f;
    //     }
    //     else
    //     {
    //         moveSpeed += 10f;
    //     }
    //
    //     rig.velocity = dir * moveSpeed;
    // }

    // bool CheckInventory()
    // {
    //     var ped = new PointerEventData(null);
    //     ped.position = Input.mousePosition;
    //     List<RaycastResult> results = new List<RaycastResult>();
    //     gr.Raycast(ped, results);
    //     if (results.Count != 0)
    //     {
    //         foreach (RaycastResult result in results)
    //         {
    //             if (result.gameObject.CompareTag("InventoryBackGround"))
    //             {
    //                 return true;
    //             }
    //         }
    //     }
    //
    //     return false;
    // }

    public void ZoomIn()
    {
        if (cam.orthographicSize >= 300)
        {
            cam.orthographicSize -= 100;
        }
        else
        {
            cam.orthographicSize = 200;
        }
    }

    public void ZoomOut()
    {
        if (cam.orthographicSize <= 900)
        {
            cam.orthographicSize += 100;
        }
        else
        {
            cam.orthographicSize = 1000;
        }
    }

    public void ResetCamrera()
    {
        transform.position = BasePosition;
        cam.orthographicSize = baseOrthographicSize;
    }
}
