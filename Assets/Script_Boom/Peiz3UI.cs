using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peiz3UI : MonoBehaviour
{
    bool Peiz3Trigger;      //Stage_System ���� Stage_Start ������ ������
    bool control;           //update ���ο��� ���� �ִϸ��̼��� �ι��̻� �������� �ʱ� ����
    Animator Anim;

    public AudioSource SFX;
    // Start is called before the first frame update
    void Start()
    {
        Peiz3Trigger = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_peiz3;
        control = false;
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stage_Start�� ����ؼ� ������
        Peiz3Trigger = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_peiz3;
        if (Peiz3Trigger == true && control == false)   //�ѹ� Stage_Start�� ���Ѵٸ�
        {
            control = true;                             //�ٽ� �ش� ���ǹ��� �ɸ��� �ʰ�
            Anim.SetBool("isPeiz3", true);              //�ִϸ��̼� ����
            SFX.Play();
        }
    }
}
