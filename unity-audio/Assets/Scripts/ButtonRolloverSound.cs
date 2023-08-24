using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles playing a sound when the mouse pointer enters a UI button's area.
/// </summary>
public class ButtonRolloverSound : MonoBehaviour, IPointerEnterHandler
{
    private AudioSource menuSFXAudioSource;

    private void Start()
    {
        // Find the AudioSource responsible for menu sound effects
        menuSFXAudioSource = GameObject.Find("MenuSFX").GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play the button rollover sound
        menuSFXAudioSource.Play();
    }
}
