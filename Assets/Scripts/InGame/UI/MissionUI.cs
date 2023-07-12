using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    public static MissionUI instance = null;

    [SerializeField] private Color color;
    [SerializeField] private Image image;
    [SerializeField] public TMP_Text text;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMission(string missionName)
    {
        if(transform.parent.gameObject.activeSelf == true)
            StartCoroutine(Twinkle(missionName));
        else
            text.text = missionName;
    }

    IEnumerator Twinkle(string missionName)
    {
        float time = 1f;
        text.text = missionName;
        image.color = Color.white;

        while (time >= 0f)
        {
            time -= Time.deltaTime * 2f;
            image.color = Color.Lerp(color, Color.white, time);
            yield return null;
        }

        image.color = color;
    }
}
