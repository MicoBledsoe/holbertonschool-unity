using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public static int previousSceneIndex;

    public AudioMixer mainMixer;
    private bool isOptionsOpen = false;
    private float normalCutoff = 22000f;  // Default high value, essentially no filter
    private float muffledCutoff = 1000f;  // Adjust this value to get the desired muffled effect

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionsMenu();
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(previousSceneIndex);
    }

    private void ToggleOptionsMenu()
    {
        isOptionsOpen = !isOptionsOpen;

        if (isOptionsOpen)
        {
            // Display options menu and muffle music
            mainMixer.SetFloat("BackgroundMusic_LowPassCutoff", muffledCutoff);
            // Additional logic to display the options menu
        }
        else
        {
            // Close options menu and restore music to normal
            mainMixer.SetFloat("BackgroundMusic_LowPassCutoff", normalCutoff);
            // Additional logic to hide the options menu
        }
    }
}