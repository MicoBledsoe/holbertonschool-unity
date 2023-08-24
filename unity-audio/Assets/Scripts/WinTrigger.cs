using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public GameObject winCanvas;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BackgroundMusicController bgmController = GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusicController>();
            bgmController.StopMusic();

            Timer otherTimer = other.GetComponent<Timer>();
            if (otherTimer != null)
            {
                float finishTime = Time.time - startTime;
                otherTimer.Win(finishTime);
                if (winCanvas != null)
                    winCanvas.SetActive(true);
            }

            // Load the next level
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0; // Loop back to the start or set to a specific scene
            }
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
