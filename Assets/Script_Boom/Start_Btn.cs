using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Btn : MonoBehaviour
{
    public AudioSource audioSoure;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startBtnPressed()
    {
        if(GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start == false)       //�̹� ���۵� ���� �Ҹ��� �鸮�� �ʵ���
            audioSoure.Play();
        GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start = true;
        
    }
}
