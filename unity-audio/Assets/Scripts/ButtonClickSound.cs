using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles playing a sound when a UI button is clicked.
/// </summary>
public class ButtonClickSound : MonoBehaviour, IPointerClickHandler
{
    private AudioSource menuSFXAudioSource;

    private void Start()
    {
        // Find the AudioSource responsible for menu sound effects
        menuSFXAudioSource = GameObject.Find("MenuSFX").GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play the button click sound
        menuSFXAudioSource.Play();
    }
}
