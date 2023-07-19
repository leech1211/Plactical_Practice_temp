using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Save : MonoBehaviour
{
    [SerializeField] private InventoryManager iManager;
    
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image icon;

    //[SerializeField] private GameObject Boom_Property; 

    private void Start()
    {
        button.onClick.AddListener(OnClick);        //onclick 리스너 추가
    }

    public void OnClick()
    {
        //RunCode 실행 결과         //outputIndex 0 은 아무 값이나 넣은 것
        List<float> result = (List<float>)BOOM_PROPERTY.instance.RunCode(0);
        if (result != null)
        {
            //공격력, 체력, 폭발시간
            GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomDamage = (int)result[0];
            GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomHealth = (int)result[1];
            GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomTime = (int)result[2];
        }
        iManager.SaveAndExit();
    }

    public void SetActive()
    {
        button.interactable = true;
        text.color = Color.white;
        icon.color = Color.white;
    }
    
    public void SetInactive()
    {
        button.interactable = false;
        text.color = new Color(1f, 1f, 1f, 0.6f);
        icon.color = text.color;

    }
    
}
