using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ambientAudioController : MonoBehaviour
{
    public AudioSource ambientAudioSource;
    public Transform playerTransform;
    public float minimumVolume = 0.1f;
    public float maximumVolume = 1.0f;
    public float fadeDistance = 5.0f;

    private void Awake()
    {
        // Assign default values to the fields if they are not set in the Inspector
        if (ambientAudioSource == null)
        {
            ambientAudioSource = GetComponent<AudioSource>();
        }
        
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
    }

    private void Update()
    {
        // Calculate the distance between the player and the ambient audio source
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Calculate the volume based on the distance
        float volume = Mathf.Lerp(minimumVolume, maximumVolume, 1.0f - Mathf.Clamp01(distanceToPlayer / fadeDistance));

        // Set the audio source volume
        ambientAudioSource.volume = volume;
    }
}
