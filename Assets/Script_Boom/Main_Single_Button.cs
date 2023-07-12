using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Single_Button : MonoBehaviour
{
    public AudioSource single_play;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextScene()
    {
        single_play.Play();
        SceneManager.LoadScene(1);
    }
}
