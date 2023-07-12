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
        //��ư���� ��쿡�� �ִϸ��̼��� ���⿡ MissingComponentException�� �߻�
        //-> �����ص� ���������..?
        /*try
        {
            if (GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health == 0)  //Stage_health �� 0�� �Ǿ��� ��
            {
                Anim.SetBool("GameOver", true);
            }
        } catch(MissingComponentException)
        {

        }*/

        // �ڱ� �ڽ��� �̹����� ��� ����
        if (Anim != null)
        {
            if (GameObject.Find("Stage_System").GetComponent<Stage_System>().Stage_health <= 0)  //Stage_health �� 0�� �Ǿ��� ��
            {
                Anim.SetBool("GameOver", true);
                //audioSoure.Play();
                Invoke("timestop", 2);          //�ִϸ��̼� �� �� �ð��� ���߱�
            }
        }

        // �ڱ� �ڽ��� ��ư�� ��� ����
        if (button != null)
        {
            // ��ư�� ���� ���� ����
        }

    }

    public void retry()
    {
        Btn1.Play();
        Time.timeScale = 1;             //�ð� �ٽ� �帣����
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Btn2.Play();
        Time.timeScale = 1;             //�ð� �ٽ� �帣����
        SceneManager.LoadScene(0);
    }

    void timestop()
    {
        Time.timeScale = 0;
    }
}
