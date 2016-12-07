using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CustomButton : Button
{
    AudioSource audioSource;
    void Start ()
    {
        this.onClick.AddListener(() => OnButtonClick());
        audioSource = gameObject.GetComponent<AudioSource>();
	}

    void OnButtonClick()
    {
        audioSource.Play();
    }
}
