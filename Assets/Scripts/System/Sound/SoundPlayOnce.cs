using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayOnce : MonoBehaviour
{
    [SerializeField] private AudioClip sound;

    public void PlaySound()
    {
        SoundManager.instance.SFXPlayOneShot("Temp", sound);
    }
}
