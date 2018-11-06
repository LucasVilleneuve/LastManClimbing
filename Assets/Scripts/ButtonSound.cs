using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{

    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}


    public void OnSelect(BaseEventData eventData)
    {
        audioSource.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.Play();
    }
}
