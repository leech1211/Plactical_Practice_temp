using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start_Timer : MonoBehaviour
{
        public int number;
        public float time;      //기다릴 시간
        private int min;        //분단위
        private float sec;      //초단위
        Text PrintText;
    
        // Start is called before the first frame update
        void Start()
        {
            time = 15;          //15초 대기 후 게임 시작
            PrintText = GetComponent<Text>();
        }
    
        // Update is called once per frame
        void Update()
        {
            // 남은 시간이 0보다 작아질 때 혹은 버튼이 눌렸을 때
            if (time <= 0 || GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start == true)
            {
                GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start = true;
                // UI 텍스트를 0초로 고정시킴.
                PrintText.text = "Start!";
            }
            else
            {
                time -= Time.deltaTime;
        
                // 전체 시간이 60초 보다 클 때
                if (time >= 60f)
                {
                    // 60으로 나눠서 생기는 몫을 분단위로 변경
                    min = (int)time / 60;
                    // 60으로 나눠서 생기는 나머지를 초단위로 설정
                    sec = time % 60;
                    // UI를 표현해준다
                    PrintText.text =  min + ":" + (int)sec;
                }
        
                // 전체시간이 60초 미만일 때
                if (time < 60f)
                {
                    // 분 단위는 필요없어지므로 초단위만 남도록 설정
                    PrintText.text = "0:" + (int)time;
                }
            }

            //PrintText.text = number.ToString();
        }
}
