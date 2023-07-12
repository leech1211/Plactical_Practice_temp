using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage_Faild_UI : MonoBehaviour
{
    Animator Anim;
    Button button;

    public AudioSource Btn1;
    public AudioSource Btn2;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        //버튼들의 경우에는 애니메이션이 없기에 MissingComponentException가 발생
        //-> 무시해도 상관없을듯..?
        /*try
        {
            if (GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health == 0)  //Stage_health 가 0이 되었을 때
            {
                Anim.SetBool("GameOver", true);
            }
        } catch(MissingComponentException)
        {

        }*/

        // 자기 자신이 이미지인 경우 동작
        if (Anim != null)
        {
            if (GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health <= 0)  //Stage_health 가 0이 되었을 때
            {
                Anim.SetBool("GameOver", true);
                //audioSoure.Play();
                Invoke("timestop", 2);          //애니메이션 이 후 시간을 멈추기
            }
        }

        // 자기 자신이 버튼인 경우 동작
        if (button != null)
        {
            // 버튼에 대한 동작 수행
        }

    }

    public void retry()
    {
        Btn1.Play();
        Time.timeScale = 1;             //시간 다시 흐르도록
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Btn2.Play();
        Time.timeScale = 1;             //시간 다시 흐르도록
        SceneManager.LoadScene(0);
    }

    void timestop()
    {
        Time.timeScale = 0;
    }
}
