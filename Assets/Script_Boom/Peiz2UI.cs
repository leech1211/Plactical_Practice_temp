using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peiz2UI : MonoBehaviour
{
    bool Peiz2Trigger;      //Stage_System 에서 Stage_Start 변수를 가져옴
    bool control;           //update 내부에서 같은 애니메이션을 두번이상 동작하지 않기 위함
    Animator Anim;

    public AudioSource SFX;
    // Start is called before the first frame update
    void Start()
    {
        Peiz2Trigger = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_peiz2;
        control = false;
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stage_Start를 계속해서 가져옴
        Peiz2Trigger = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_peiz2;
        if (Peiz2Trigger == true && control == false)   //한번 Stage_Start가 변한다면
        {
            control = true;                             //다시 해당 조건문에 걸리지 않게
            Anim.SetBool("isPeiz2", true);              //애니메이션 시작
            SFX.Play();
        }
    }
}
