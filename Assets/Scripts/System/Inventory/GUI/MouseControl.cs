using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MouseControl : MonoBehaviour
{
    enum State { Empty, Menu, Node, Button, Flow, ToolBar };
    public GameObject menu_Base;
    public GameObject menu_Node;
    public GameObject addNode;
    public GameObject intNode;
    public Transform canvas;
    public GraphicRaycaster[] graphicRaycasters;

    public List<GameObject> selectedObject;

    public RectTransform dragBox;
    bool dragFlag;
    Vector3 currentPoint;
    Vector3 dragPoint_Start;
    Vector3 dragPoint_End;
    Rect dragRect;

    ItemListUI _itemListUI;

    void Start()
    {
        menu_Base.SetActive(false);
        dragFlag = false;
        _itemListUI = GameObject.Find("Inventory").GetComponent<ItemListUI>();
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
                menu_Node.GetComponent<RectTransform>().position = currentPoint;
                menu_Base.SetActive(false);
                menu_Node.SetActive(true);
            }
            else if (clickState == State.Button)
            {
                menu_Base.SetActive(false);
                menu_Node.SetActive(false);
            }
            else if (clickState == State.Flow)
            {
                menu_Base.SetActive(false);
                menu_Node.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            State clickState = CheckIn();
            if (clickState == State.Empty)
            {
                ButtonSelect(false);
                selectedObject.Clear();
                menu_Base.SetActive(false);
                menu_Node.SetActive(false);
                dragPoint_Start = Input.mousePosition;
                dragRect = new Rect();
                dragFlag = true;
            }
            if (clickState == State.Node)
            {
                menu_Base.SetActive(false);
                menu_Node.SetActive(false);
            }
        }

        if (dragFlag == true)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                dragPoint_End = Input.mousePosition;
                DrawDragBox();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                SelectNodes();
                dragPoint_Start = Vector2.zero;
                dragPoint_End = Vector2.zero;
                DrawDragBox();
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
                    if (result.gameObject.tag == "Menu")
                    {
                        return State.Menu;
                    }
                    else if (result.gameObject.tag == "Node")
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
                    else if (result.gameObject.tag == "Button")
                    {
                        ButtonSelect(false);
                        selectedObject.Clear();
                        selectedObject.Add(result.gameObject);
                        ButtonSelect(true);
                        return State.Button;
                    }
                    else if (result.gameObject.tag == "Flow")
                    {
                        ButtonSelect(false);
                        selectedObject.Clear();
                        selectedObject.Add(result.gameObject);
                        ButtonSelect(true);
                        return State.Flow;
                    }
                    else if (result.gameObject.tag == "ToolBar")
                    {
                        return State.ToolBar;
                    }
                }
            }
        }
        return State.Empty;
    }

    public void CreateAdd()
    {
        GameObject node = Instantiate(addNode, currentPoint, Quaternion.identity);
        node.transform.SetParent(canvas);
        menu_Base.SetActive(false);
    }

    public void CreateInt()
    {
        GameObject node = Instantiate(intNode, currentPoint, Quaternion.identity);
        node.transform.SetParent(canvas);
        menu_Base.SetActive(false);
    }

    public void DeleteNode()
    {
        foreach (GameObject tempNodeObject in selectedObject)
        {
            Node tempNode = tempNodeObject.GetComponent<Node>();

            // if (tempNode.NODE_TYPE == NodeType.START || tempNode.NODE_TYPE == NodeType.SHOOT)
            // {
            //     tempNodeObject.GetComponent<Draggable>().SetNormal();
            //     continue;
            // }
            for (int i = 0; i < tempNode.inputButton.Length; i++)
            {
                GameObject input = tempNode.inputButton[i];
                if (input != null)
                {
                    input.GetComponent<LineManager>().DisconnectLine(tempNodeObject.transform.GetChild(0).GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < tempNode.outputButton.Count; i++)
            {
                if (tempNode.outputButton[i].Count != 0)
                {
                    tempNodeObject.transform.GetChild(1).GetChild(i).GetComponent<LineManager>().DisconnectLineAll();
                }
            }

            for (int i = 0; i < tempNode.inputFlow.Count; i++)
            {
                GameObject input = tempNode.inputFlow[i];
                if (input != null)
                {
                    input.GetComponent<LineFlowManager>().DisconnectLine();
                }
            }

            for (int i = 0; i < tempNode.outputFlow.Count; i++)
            {
                if (tempNode.outputFlow[i] != null)
                {
                    tempNodeObject.transform.GetChild(3).GetChild(i).GetComponent<LineFlowManager>()
                        .DisconnectLine();
                }
            }

            _itemListUI.AddItem(tempNodeObject);

            Destroy(tempNodeObject);
        }
        menu_Node.SetActive(false);
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
                selectedObject.Add(node.gameObject);
            }
        }
        ButtonSelect(true);
    }

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
}