using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TimeScore : MonoBehaviour
{
    //[SerializeField] private GameNetWorkManager gameNetWorkManager;
    [SerializeField] private TMP_Text timeUI;
    
    [SerializeField] private Color redColor;

    [SerializeField] private bool hide;

    private float time;
    private int min;
    private float sec;

    private void Start()
    {
        /*if(gameNetWorkManager == null)
            gameNetWorkManager = GameNetWorkManager.instance;*/
        
        //timeUI.text = "Coding!";
    }

    private void Update()
    {
        /*if(gameNetWorkManager == null)
            return;
        time = gameNetWorkManager.time.Value;
        
        
        if (hide && gameObject.activeSelf && time <= 0)
        {
            gameObject.SetActive(false);
            return;
        }*/
        
        time = Stage_System.instance.stage_time;
        
        // 남은 시간이 0보다 작아질 때 혹은 버튼이 눌렸을 때
        if (time <= 0 || Stage_System.instance.Stage_Start == true)
        {
            timeUI.text = "Playing!";
        }
        else
        {
            if (time > 30)
                timeUI.color = Color.white;
            else
                timeUI.color = redColor;

            min = (int)(time / 60);
            sec = (int)(time % 60);

            // 전체 시간이 60초 보다 클 때
            if (time >= 60f)
            {
                // 60으로 나눠서 생기는 몫을 분단위로 변경
                min = (int)time / 60;
                // 60으로 나눠서 생기는 나머지를 초단위로 설정
                sec = time % 60;
                // UI를 표현해준다
                timeUI.text = min + ":" + (int)sec;
            }

            // 전체시간이 60초 미만일 때
            if (time < 60f)
            {
                // 분 단위는 필요없어지므로 초단위만 남도록 설정
                timeUI.text = "0:" + (int)time;
            }
        }

        
    }
}
