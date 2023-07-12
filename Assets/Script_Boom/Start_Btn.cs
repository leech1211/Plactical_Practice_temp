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
        if(GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start == false)       //이미 시작된 경우는 소리가 들리지 않도록
            audioSoure.Play();
        GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Start = true;
        
    }
}
