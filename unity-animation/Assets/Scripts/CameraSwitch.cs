using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the main camera that follows the player
    public Camera cutsceneCamera;  // Reference to the cutscene camera

    // Start is called before the first frame update
    void Start()
    {
        // Disable the cutscene camera at the start
        cutsceneCamera.gameObject.SetActive(false);
    }

    // Function to switch to the main camera
    public void SwitchToMainCamera()
    {
        mainCamera.gameObject.SetActive(true);
        cutsceneCamera.gameObject.SetActive(false);
    }
}
