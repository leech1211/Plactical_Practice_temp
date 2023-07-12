using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    bool startTrigger;      //Stage_System ���� Stage_Start ������ ������
    bool control;           //update ���ο��� ���� �ִϸ��̼��� �ι��̻� �������� �ʱ� ����
    Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        startTrigger = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start;
        control = false;
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stage_Start�� ����ؼ� ������
        startTrigger = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start;
        if (startTrigger == true && control == false)   //�ѹ� Stage_Start�� ���Ѵٸ�
        {
            control = true;                             //�ٽ� �ش� ���ǹ��� �ɸ��� �ʰ�
            Anim.SetBool("isStart", true);              //�ִϸ��̼� ����
        }       
    }
}
