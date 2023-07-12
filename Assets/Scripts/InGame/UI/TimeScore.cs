using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TimeScore : MonoBehaviour
{
    [SerializeField] private GameNetWorkManager gameNetWorkManager;
    [SerializeField] private TMP_Text timeUI;
    
    [SerializeField] private Color redColor;

    [SerializeField] private bool hide;

    private float time;
    private int min;
    private int sec;

    private void Start()
    {
        if(gameNetWorkManager == null)
            gameNetWorkManager = GameNetWorkManager.instance;
    }

    private void Update()
    {
        if(gameNetWorkManager == null)
            return;
        time = gameNetWorkManager.time.Value;

        if (hide && gameObject.activeSelf && time <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

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
    }
}
