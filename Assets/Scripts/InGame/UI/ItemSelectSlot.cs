using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSelectSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image myImage;
    [SerializeField] private List<Image> otherImage;
    [SerializeField] private Button selectButton;
    [SerializeField] private AudioClip sound;
    
    public NodeType item;

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.instance.SFXPlayOneShot("Temp", sound);
        myImage.enabled = true;
        foreach (var other in otherImage)
        {
            other.enabled = false;
        }

        selectButton.interactable = true;
    }
}
