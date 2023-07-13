using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private void Awake()
    {
        instance = this;
    }

    enum State { Empty, Node, Button, Flow, ToolBar };
    [SerializeField] private Transform canvas;
    [SerializeField] private List<GraphicRaycaster> graphicRaycasters;
    [SerializeField] public List<GameObject> selectedObject;
    [SerializeField] private InventoryCameraManager cameraManager;
    [SerializeField] private string sceneName = "Inventory";

    public RectTransform dragBox;
    bool dragFlag;
    Vector3 currentPoint;
    Vector3 dragPoint_Start;
    Vector3 dragPoint_End;
    Rect dragRect;

    ItemListUI _itemListUI;

    void Start()
    {
        dragFlag = false;
        _itemListUI = GameObject.Find("Frame").GetComponent<ItemListUI>();
        InventoryField.instance.Initialize();
        canvas = InventoryField.instance.transform;
        graphicRaycasters[1] = InventoryField.instance.GetComponent<GraphicRaycaster>();
        ItemStorage.instance.InventoryUI = transform;
        InventoryField.instance.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            State clickState = CheckIn();
            if (clickState == State.Node)
            {
                currentPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -Camera.main.transform.position.z));
                currentPoint = new Vector3(currentPoint.x, currentPoint.y, 90);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            State clickState = CheckIn();
            if (clickState == State.Empty)
            {
                ButtonSelect(false);
                selectedObject.Clear();
                // dragPoint_Start = Input.mousePosition;
                // dragRect = new Rect();
                cameraManager.SetMoveCamera();
                dragFlag = true;
            }
        }

        if (dragFlag == true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                cameraManager.UpdateMoveCamera();
                
                // dragPoint_End = Input.mousePosition;
                // DrawDragBox();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                
                
                // SelectNodes();
                // dragPoint_Start = Vector2.zero;
                // dragPoint_End = Vector2.zero;
                // DrawDragBox();
                dragFlag = false;
            }
        }
    }

    State CheckIn()
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        foreach (GraphicRaycaster gr in graphicRaycasters)
        {
            gr.Raycast(ped, results);
            if (results.Count != 0)
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("Node"))
                    {
                        if (selectedObject.Contains(result.gameObject) == false)
                        {
                            ButtonSelect(false);
                            selectedObject.Clear();
                            selectedObject.Add(result.gameObject);
                            ButtonSelect(true);
                        }
                        return State.Node;
                    }
                    else if (result.gameObject.CompareTag("Node_Button"))
                    {
                        ButtonSelect(false);
                        selectedObject.Clear();
                        selectedObject.Add(result.gameObject);
                        ButtonSelect(true);
                        return State.Button;
                    }
                    else if (result.gameObject.CompareTag("Node_Flow"))
                    {
                        ButtonSelect(false);
                        selectedObject.Clear();
                        selectedObject.Add(result.gameObject);
                        ButtonSelect(true);
                        return State.Flow;
                    }
                    else if (result.gameObject.CompareTag("InventoryUI"))
                    {
                        return State.ToolBar;
                    }
                }
            }
        }
        return State.Empty;
    }
    
    public void DeleteNode(GameObject tempNodeObject)
    {
        Node tempNode = tempNodeObject.GetComponent<Node>();
        int count;
        if (tempNode.isStatic)
        {
            tempNodeObject.GetComponent<Draggable>().SetNormal();
            return;
        }

        count = tempNode.inputButton.Length;
        for (int i = 0; i < count; i++)
        {
            GameObject input = tempNode.inputButton[i];
            if (input != null)
            {
                input.GetComponent<LineManager>().DisconnectLine(tempNodeObject.transform.GetChild(0).GetChild(i).gameObject);
            }
        }

        count = tempNode.outputButton.Count;
        for (int i = 0; i < count; i++)
        {
            if (tempNode.outputButton[0].Count != 0)
            {
                tempNodeObject.transform.GetChild(1).GetChild(i).GetComponent<LineManager>().DisconnectLineAll();
            }
        }

        count = tempNode.inputFlow.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject input = tempNode.inputFlow[0];
            if (input != null)
            {
                input.GetComponent<LineFlowManager>().DisconnectLine();
            }
        }

        count = tempNode.outputFlow.Count;
        for (int i = 0; i < count; i++)
        {
            if (tempNode.outputFlow[0] != null)
            {
                tempNodeObject.transform.GetChild(3).GetChild(i).GetComponent<LineFlowManager>()
                    .DisconnectLine();
            }
        }

        _itemListUI.AddItem(tempNodeObject);

        Destroy(tempNodeObject);
        selectedObject.Clear();
    }

    #region OldCode(DeleteCode)
    public void DeleteNode()
    {
        foreach (GameObject tempNodeObject in selectedObject)
        {
            Node tempNode = tempNodeObject.GetComponent<Node>();
            int count;
            if (tempNode.isStatic)
            {
                tempNodeObject.GetComponent<Draggable>().SetNormal();
                continue;
            }

            count = tempNode.inputButton.Length;
            for (int i = 0; i < count; i++)
            {
                GameObject input = tempNode.inputButton[i];
                if (input != null)
                {
                    input.GetComponent<LineManager>().DisconnectLine(tempNodeObject.transform.GetChild(0).GetChild(i).gameObject);
                }
            }

            count = tempNode.outputButton.Count;
            for (int i = 0; i < count; i++)
            {
                if (tempNode.outputButton[0].Count != 0)
                {
                    tempNodeObject.transform.GetChild(1).GetChild(i).GetComponent<LineManager>().DisconnectLineAll();
                }
            }

            count = tempNode.inputFlow.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject input = tempNode.inputFlow[0];
                if (input != null)
                {
                    input.GetComponent<LineFlowManager>().DisconnectLine();
                }
            }

            count = tempNode.outputFlow.Count;
            for (int i = 0; i < count; i++)
            {
                if (tempNode.outputFlow[0] != null)
                {
                    tempNodeObject.transform.GetChild(3).GetChild(i).GetComponent<LineFlowManager>()
                        .DisconnectLine();
                }
            }

            _itemListUI.AddItem(tempNodeObject);

            Destroy(tempNodeObject);
        }
        selectedObject.Clear();
    }
    
    void DrawDragBox()
    {
        dragBox.position = (dragPoint_Start + dragPoint_End) * 0.5f;
        dragBox.sizeDelta = new Vector2(Mathf.Abs(dragPoint_Start.x - dragPoint_End.x),
            Mathf.Abs(dragPoint_Start.y - dragPoint_End.y));
    }

    void SelectNodes()
    {
        Vector3 startPos = Camera.main.ScreenToWorldPoint(new Vector3(dragPoint_Start.x,
            dragPoint_Start.y, -Camera.main.transform.position.z));
        Vector3 endPos = Camera.main.ScreenToWorldPoint(new Vector3(dragPoint_End.x,
            dragPoint_End.y, -Camera.main.transform.position.z));

        if (endPos.x < startPos.x)
        {
            dragRect.xMin = endPos.x;
            dragRect.xMax = startPos.x;
        }
        else
        {
            dragRect.xMin = startPos.x;
            dragRect.xMax = endPos.x;
        }
        if (endPos.y < startPos.y)
        {
            dragRect.yMin = endPos.y;
            dragRect.yMax = startPos.y;
        }
        else
        {
            dragRect.yMin = startPos.y;
            dragRect.yMax = endPos.y;
        }

        selectedObject.Clear();
        for (int i = 0; i < canvas.childCount; i++)
        {
            Transform node = canvas.GetChild(i);
            if (dragRect.Contains(node.position))
            {
                if (node.GetComponent<Node>().isStatic == false)
                {
                    selectedObject.Add(node.gameObject);
                }
            }
        }
        ButtonSelect(true);
    }
    #endregion

    void ButtonSelect(bool flag)
    {
        float alpha = 1f;
        if (flag == false)
        {
            alpha = 0.7f;
        }
        foreach (GameObject nodeObject in selectedObject)
        {
            if (nodeObject.TryGetComponent<Button>(out Button button))
            {
                if (flag == true)
                {
                    nodeObject.GetComponent<Draggable>().SetSelect();
                }
                else
                {
                    nodeObject.GetComponent<Draggable>().SetNormal();
                }

                Node tempNode = nodeObject.GetComponent<Node>();
                if (tempNode.isStatic)
                {
                    continue;
                }

                ColorBlock colorBlock = button.colors;
                colorBlock.normalColor = new Color(1, 1, 1, alpha);
                button.colors = colorBlock;

            }
        }
    }

    public void SaveAndExit()       //청사진에서 돌아오는 코드
    {
        CodingButton.instance.readyToReturnScene();     //돌아올 때 UI,시간 등 처리
        ItemStorage.instance.ChangeField("Inventory");
        InventoryField.instance.gameObject.SetActive(false);
        SceneManager.UnloadSceneAsync(sceneName);       //더해진 씬(Inventory) 내리기
    }

    public void Reset()
    {
        cameraManager.ResetCamrera();
    }
}
