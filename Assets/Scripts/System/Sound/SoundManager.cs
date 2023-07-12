using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SFXPlay(string sfxName, AudioClip clip, float vol = 0.5f)
    {
        GameObject go = new GameObject(sfxName + "Sound");

        AudioSource audiosource = go.AddComponent<AudioSource>();

        audiosource.volume = vol;

        audiosource.clip = clip;
        
        audiosource.Play();
        
        Destroy(go, clip.length);
    }
    
    public AudioSource SFXPlayLoop(string sfxName, AudioClip clip, float vol = 0.5f)
    {
        GameObject go = new GameObject(sfxName + "Sound");

        AudioSource audiosource = go.AddComponent<AudioSource>();

        audiosource.volume = vol;

        audiosource.clip = clip;

        audiosource.loop = true;
        
        audiosource.Play();
        
        return audiosource;
    }

    public AudioSource SFXPlayLoopBS(string sfxName, AudioClip clip, float vol = 0.5f)
    {
        GameObject go = new GameObject(sfxName + "Sound");

        AudioSource audiosource = go.AddComponent<AudioSource>();
        
        DontDestroyOnLoad(audiosource);

        audiosource.volume = vol;

        audiosource.clip = clip;

        audiosource.loop = true;
        
        audiosource.Play();
        
        return audiosource;
    }
    
    public void SFXPlayOneShot(string sfxName, AudioClip clip, float vol = 0.5f)
    {
        GameObject go = new GameObject(sfxName + "Sound");

        AudioSource audiosource = go.AddComponent<AudioSource>();

        audiosource.volume = vol;
        
        audiosource.PlayOneShot(clip);
        
        Destroy(go, clip.length);
    }



    public void SFXPlayOneShotBS(string sfxName, AudioClip clip, float vol = 0.5f)
    {
        GameObject go = new GameObject(sfxName + "Sound");

        AudioSource audiosource = go.AddComponent<AudioSource>();

        DontDestroyOnLoad(audiosource);

        audiosource.volume = vol;
        
        audiosource.PlayOneShot(clip);
        
        Destroy(go, clip.length);
    }


}

