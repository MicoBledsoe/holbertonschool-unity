using UnityEngine;

public class TimeController : MonoBehaviour
{
    public void StopTime()
    {
        Time.timeScale = 0f; // Stops time
    }
}
