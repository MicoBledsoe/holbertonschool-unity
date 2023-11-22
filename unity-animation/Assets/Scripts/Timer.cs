using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public Text TimerScreenText; //The text that is shown on screen in the game
    private float elapsedTimeInGame; //Elasped time in game
    private bool timerActive = true;
    public float stopHeight = -10f; //The threshold to stop the TimerScreenText
    public Transform OG_StartPosition; //Referencing the player's transform

    private void Start()
    {
        // resetting the elapsed time
        elapsedTimeInGame = 0f;
        
        if(TimerScreenText == null)
        {
            Debug.LogError("MyScreenText is not set");
            return; // or just handle the null case
        }
    }

    void OnDisable()
    {
        if(TimerScreenText != null)
        {
        TimerScreenText.color = Color.green; // The TimerScreenText color
        TimerScreenText.fontSize = 50; //sizing of the text
        }
    }

    private void Update() //updating every frame
    {
        if(timerActive) //this updates the timer only if its active !!!
        {

        // Update the elapsed time
        elapsedTimeInGame += Time.deltaTime;

        // This Calculates the minutes, seconds, and milliseconds during the game constantly
        int minutes = (int)(elapsedTimeInGame / 60f); //caculatin 
        int seconds = (int)(elapsedTimeInGame % 60f); //caculating the seconds
        int milliseconds = (int)((elapsedTimeInGame * 100f) % 100f); //caculating the milliseconds

        // Update the TimerText object with the formatted time which will appear on to the screen
        TimerScreenText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
        }
    }
}