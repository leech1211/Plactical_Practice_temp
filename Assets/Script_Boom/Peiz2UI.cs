using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peiz2UI : MonoBehaviour
{
    bool Peiz2Trigger;      //Stage_System ���� Stage_Start ������ ������
    bool control;           //update ���ο��� ���� �ִϸ��̼��� �ι��̻� �������� �ʱ� ����
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
        //Stage_Start�� ����ؼ� ������
        Peiz2Trigger = GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_peiz2;
        if (Peiz2Trigger == true && control == false)   //�ѹ� Stage_Start�� ���Ѵٸ�
        {
            control = true;                             //�ٽ� �ش� ���ǹ��� �ɸ��� �ʰ�
            Anim.SetBool("isPeiz2", true);              //�ִϸ��̼� ����
            SFX.Play();
        }
    }
}
