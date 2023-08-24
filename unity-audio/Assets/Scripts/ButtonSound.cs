using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles playing rollover and click sounds for a UI button.
/// </summary>
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioSource menuSFXAudioSource;
    public AudioClip rolloverClip;
    public AudioClip clickClip;

    private void Start()
    {
        // Find the AudioSource responsible for menu sound effects
        menuSFXAudioSource = GameObject.Find("MenuSFX").GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play the button rollover sound
        menuSFXAudioSource.PlayOneShot(rolloverClip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play the button click sound
        menuSFXAudioSource.PlayOneShot(clickClip);
    }
}
