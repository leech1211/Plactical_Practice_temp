using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnNode : MonoBehaviour
{

    public Color hoverColor;
    private GameObject UserNPC;     //유저의 NPC
    private Renderer rend;
    private Color startColor;

    private int Stage_Money;        //유저의 돈
    int Boom_price;                //Boom의 가격
    bool stage_start;

    void Start()
    {
        stage_start = false;
        UserNPC = null;
        //Stage_System의 변수를 올바르게 가져오기 위해 1초 뒤 초기화(start와 같은 역할)
        Invoke("init", 1);
    }
    
    void init()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        Stage_Money = GameObject.Find("Stage_System").GetComponent<Stage_System>().money;
        Boom_price = GameObject.Find("Stage_System").GetComponent<Stage_System>().BoomPrice;
    }

    void Update()
    {
        //NPC가 살아있으면 빨간색 + 생성불가
        //NPC가 없는데 마우스가 올라오면 초록색
        //그이외의 경우는 회색

        if (UserNPC != null)
        {
            rend.material.color = Color.red;    //빨간 표시
        }
        else
        {
            if (rend != null) // rend 객체도 Null인지 체크해야 함
            {
                rend.material.color = hoverColor;   //초록 표시
            }
        }

        Stage_Money = GameObject.Find("Stage_System").GetComponent<Stage_System>().money;
        stage_start = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start;
    }

  
    void OnMouseDown()
    {
        Debug.Log("click");
        if (UserNPC != null)
        {
            //Debug.Log("여기는 지을 수 없다");
            return;
        }
        //build a turret

        if(Stage_Money < Boom_price)       //돈이 부족하다면
        {
            //Debug.Log("구매 불가");
            return;
        }
        else            //돈이 있다면
        {
            if(stage_start == true)         //게임이 시작한 경우라면
            {
                GameObject.Find("Stage_System").GetComponent<Stage_System>().money -= Boom_price;               //유저 돈 감소
            }
            GameObject UserNPCtoBuild = Stage_System.instance.GetUserNPCtoBuild();              
            UserNPC = (GameObject)Instantiate(UserNPCtoBuild, transform.position, transform.rotation);      //Boom 스폰
        }
        
    }

   

  

}