using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeizBlink : MonoBehaviour
{
    public bool stage_start;
    private float time;
    private float size;
    private float upSizeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        stage_start = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start;
    }

    // Update is called once per frame
    void Update()
    {
        stage_start = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start;
        if (stage_start == true)       // 게임이 진행되고 있다면 보이도록
        {
            this.gameObject.SetActive(false);
        }
        else                   // 게임이 진행되고 있지 않다면 보이지 않도록
        {
            this.gameObject.SetActive(true);
        }
    }
}