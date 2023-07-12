using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_Boom: MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource clearSFX;
    public AudioSource failSFX;
    bool BGMControl;
    // Start is called before the first frame update
    void Start()
    {
        bgm.Play();
        BGMControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Fail == true && BGMControl == false)
        {
            BGMControl = true;
            bgm.Stop();             //스테이지 실패시 브금 정지
            failSFX.Play();         //실패 효과음
        }
        else if (GameObject.Find("Stage_System").GetComponent<Stage_System>().PrintNum <= 0 && BGMControl == false
            && GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_Fail == false)
        {
            BGMControl = true;
            bgm.Stop();             //스테이지 성공시 브금 정지
            clearSFX.Play();        //스테이지클리어 효과음
        }
    }
}
