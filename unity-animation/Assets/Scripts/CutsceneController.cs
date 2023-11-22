using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public GameObject timerCanvas;
    public GameObject MainCamera;
    public GameObject player;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //Grabbing Animator Component
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            // Enabling Main Cam
            MainCamera.SetActive(true);

            //Verifying player is getting enabled with a debug
            Debug.Log("Enabling player...");
            //Enabling Player Controll
            player.GetComponent<PlayerController>().enabled = true;
            this.gameObject.SetActive(false);

            //Enabling Timer
            timerCanvas.SetActive(true);

            //Referecing back to the class which is CutScence, in which i will be disabling the cutscene controller script
            this.enabled = false;
        }
    }
}
