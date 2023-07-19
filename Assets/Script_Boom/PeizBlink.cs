using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeizBlink : MonoBehaviour
{
    private bool stage_start;
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
        
        if (stage_start)       // 타이머가 돌아가고 있을 때는 비활성화
        {
            this.gameObject.SetActive(false);
        }
        else                   // 타이머가 돌아가지 않을 때는 활성화하여 보이게 함
        {
            this.gameObject.SetActive(true);
        }
    }
}