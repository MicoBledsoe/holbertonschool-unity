using UnityEngine;

public class MuffledSoundController : MonoBehaviour
{
    public AudioSource muffledAudioSource;
    public AudioClip muffledClip;
    public AudioClip originalClip;

    private bool isMuffled = false;

    private void Start()
    {
        muffledAudioSource.clip = originalClip;
    }

    public void ToggleMuffledSound()
    {
        isMuffled = !isMuffled;

        if (isMuffled)
        {
            muffledAudioSource.clip = muffledClip;
        }
        else
        {
            muffledAudioSource.clip = originalClip;
        }

        muffledAudioSource.Play();
    }
}
