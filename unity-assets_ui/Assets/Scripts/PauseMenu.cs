using UnityEngine;
using UnityEngine.SceneManagement;
// Directives ^

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas; // Reference to the pause menu canvas in the scene.
    private bool isGamePaused = false; // Tracks whether the game is currently paused.

    private void Update() //updating every fram constantly
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Checking to see if the Escape keybind is pressed !
        {
            if (isGamePaused) //if game is paused
            {
                Resume(); // If the game is paused, resume it.
            }
            else
            {
                Pause(); // If the game is not paused, pause it.
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f; // Pause the game by setting time scale to 0.
        isGamePaused = true; // Set the isPaused flag to true.
        pauseCanvas.SetActive(true); // Shows the pause menu canvas.
    }

    public void Resume()
    {
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1.
        isGamePaused = false; // Set the isPaused flag to false.
        pauseCanvas.SetActive(false); // Hides the pause menu canvas.
    }

    public void Restart()//Restart 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene for a restart.
        Time.timeScale = 1f; // Ensure the time scale is set to 1 to resume normal play.
    }

    public void MainMenu() //Main Menu
    {
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene.
        Time.timeScale = 1f; // Reset the time scale to normal upon exiting to the menu
    }

    public void Options() //Options menu
    {
        SceneManager.LoadScene("Options"); // Load the options scene.
        Time.timeScale = 1f; // Ensuring game time is back to normal when entering the options.
    }
}
