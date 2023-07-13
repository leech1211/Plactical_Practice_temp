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
    private int sec;

    private void Start()
    {
        /*if(gameNetWorkManager == null)
            gameNetWorkManager = GameNetWorkManager.instance;*/
        //코딩중에는 시간이 멈춘것으로 하겠다
        timeUI.text = "Coding!";
    }

    private void Update()
    {
        /*if(gameNetWorkManager == null)
            return;
        time = gameNetWorkManager.time.Value;
        time = Stage_System.instance.stage_time;
        
        if (hide && gameObject.activeSelf && time <= 0)
        {
            gameObject.SetActive(false);
            return;
        }*/
        
        
        
        /*// 남은 시간이 0보다 작아질 때 혹은 버튼이 눌렸을 때
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

            if (min < 10)
            {
                timeUI.text = $"0{min}:";
            }
            else
            {
                timeUI.text = $"{min}:";
            }

            if (sec < 10)
            {
                timeUI.text += $"0{sec}";
            }
            else
            {
                timeUI.text += $"{sec}";
            }
        }*/

        
    }
}
