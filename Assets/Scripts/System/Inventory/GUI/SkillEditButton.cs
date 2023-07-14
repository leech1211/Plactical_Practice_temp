using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillEditButton : MonoBehaviour, IPointerClickHandler      //노드의 스킬수정 버튼에 들어가는 스크립트
{
    [SerializeField] private string FieldName;
    [SerializeField] private List<GameObject> Preset;
    [SerializeField] private List<Vector3> PresetPosition;

    private bool isInitialize;
    
    private void OnEnable()
    {
        isInitialize = false;
    }

    private void Update()
    {
        if (isInitialize == false)
        {
            if (!ItemStorage.instance.HasField(FieldName))      //ItemStorage에 FieldName 필드가 없다면
            {
                //필드 생성
                GameObject newField = new GameObject();
                newField.name = FieldName;
                newField.transform.position = new Vector3(960f, 540f, -201f);
                newField.transform.parent = ItemStorage.instance.transform;

                int i = 0;
                //foreach문 뭐지
                //foreach문 없으면 씬 잘 전환되지만 Transform child out of bounds 오류
                foreach (var presetNode in Preset)
                {
                    GameObject tempNode = Instantiate(presetNode);      //프리셋 오브젝트 생성
                    tempNode.transform.parent = newField.transform;     //위치 잡아주기?
                    tempNode.GetComponent<RectTransform>().anchoredPosition3D = PresetPosition[i];      //위치잡아주기?

                    if (i == 0)
                    {
                        transform.parent.parent.GetComponent<Node_Skill>().propertyObject = tempNode;
                        //Node_Skill이 생성되지 않은 건가
                    }
                    
                    i++;
                }
                ItemStorage.instance.AddField(FieldName, newField);         //만든 newField를 Dictionary에 추가
            }
            else if(Preset.Count != 0)
            {
                transform.parent.parent.GetComponent<Node_Skill>().propertyObject =
                    ItemStorage.instance.GetField(FieldName).GetChild(0).gameObject;
            }

            isInitialize = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemStorage.instance.ChangeField(FieldName);
    }
}
