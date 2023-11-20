using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//my directives above ^

public class PlayerController : MonoBehaviour //playercontroller class inherting from the mono which is a base class from unity
{
    public float Speed = 5f; //Movement Speed in which i can edit within unitys inspector window
    public float jumpForceUp = 5f; //The jump force applied in which i can edit within unitys inspector window as well
    public float RespawnHeight = -10f; //Respawn Height
    public float RespawnOffset = 2f; //Respawn Offset
    private Rigidbody RB;
    public Vector3 OG_StartPosition; //This is for marking the starting position in which is a reference to the Transform component
    public float rotationSpeed = 700f; //This is for the rotation of OGPlayer
    public Animator animator; // Animator component controlling the OGPlayer's animation states and transitions.
    private bool isOGGrounded; //Keeping track if OGPlayer is grounded
    private float jumpTimeCounter; //Time tracked since jump
    private bool justJumped = false;

    //ensuring that necessary references are set up before the game starts
    private void Awake() //This lifecycle method is called when the instance is being loaded
    {
        RB = GetComponent<Rigidbody>(); //assigns the rigidbody component to the var RB !
        animator = GetComponent<Animator>(); // Retrieves the Animator component attached to this GameObject, also used to manage animation states and transitions !
        
        OG_StartPosition = new Vector3(0f, 0.358517f, 0f);
    }

    private void OnCollisionEnter(Collision OGCollision) //Enter Collision when OG comes in contact with another object
    {
        if(OGCollision.gameObject.tag == "Ground") //Checking if the collision is with a GameObject tagged as "Ground" in the inspector
        {
            isOGGrounded = true;
            animator.SetBool("OGHasLanded", true); //Set the Animator parameter OGHasLanded to true, indicating the OGPlayer has landed on the ground

            if(animator.GetBool("OGPlayerFalling"))
            {
                animator.SetTrigger("OGImpacted"); //Triggering the OG Impact
            }
            animator.SetBool("OGPlayerFalling", false);
        }
    }
    
    private void OnCollisionExit(Collision OGCollision) //Exit Collision when OG stops colliding with another object
    {
        if(OGCollision.gameObject.tag == "Ground") //Checking if the collision that ended is with a GameObject tagged as "Ground" in the inspector
        {
            isOGGrounded = false;
            animator.SetBool("OGHasLanded", false); //Checking the Animator parameter OGHasLanded to false, indicating the OPlayer is no longer on the ground
        }
    }

    private void RespawnOGPlayer() //created my own method and obv void means it doesnt return any value to it 
    {
    Debug.LogError("Respawning player...");

    // Resets the OG player's velocity back to zero to stop movement
    RB.velocity = Vector3.zero;
    Debug.LogError("Velocity set to zero! You are in the clear.");

    // Disabling gravity for the player
    RB.useGravity = false;
    Debug.LogError("Gravity is disabled! You are in the clear.");

    // Move the player back to the OG position with an offset
    transform.position = OG_StartPosition + Vector3.up * RespawnOffset;
    Debug.LogError("Player has respawned back to the OG position.");

    // Re-enable gravity for the player
    RB.useGravity = true;
    Debug.LogError("Gravity is re-enabled! You are in the clear.");
    }

    private void Update() //Constantly updating every frame for any input by the user that happens within the game
    {
        float XMovement = Input.GetAxis("Horizontal"); //Grabs/checks the players horizontal inputs like the 'A' and 'D' keybinds -- LEFT/RIGHT
        float ZMovement = Input.GetAxis("Vertical"); //Grabs/checks the players Vertical inputs like the 'W' and 'S' keybinds -- FORWARD/BACKWARD

        Vector3 Movement = new Vector3(XMovement, 0f, ZMovement) * Speed; //caculating the movement direction according when the player gives input
        RB.velocity = new Vector3(Movement.x, RB.velocity.y, Movement.z);//this sets the Rigidbodys horizontal velocity based on players input.

        //Figures out if the OGPlayer is moving
        bool isMovingOGPlayer = XMovement != 0 || ZMovement != 0; //boolean var, set to true if either X or Y is true if its zero then that means OG is stationary
        animator.SetBool("IsRunning", isMovingOGPlayer); // Sets the 'IsRunning' Animator parameter to true if the player is moving, false if not otherwise !
        
        //Figures out if the OGPlayer is falling
        bool OGPlayerFalling = (RB.velocity.y < -0.5f) && !isOGGrounded && !justJumped; //0.5 is the threshold for falling !
        animator.SetBool("OGPlayerFalling", OGPlayerFalling);
        Debug.Log("Velocity Y: " + RB.velocity.y + ", isOGGrounded: " + isOGGrounded + ", OGPlayerFalling: " + OGPlayerFalling);

        if(justJumped &&(Time.time - jumpTimeCounter > 0.2f)) //0.2 seconds of delay
        {
            justJumped = false;
        }

        // Rotation logic
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; // calculates a rotation value based on OGplayer's horizontal input
        transform.Rotate(0, rotation, 0); // This rotates the OGPlayer on the Y axis


        if(Input.GetKeyDown(KeyCode.T)) //if the 'T' keybind input is being pressed
        {
            //FORCE RESPAWN OGPlayer at any moment in the game if the 'T' keybind is pressed anywhere the OGPlayer is located or falling off the map works
            RespawnOGPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Space)) //if the space bar keybind is being pressed
        {
            RB.AddForce(Vector3.up * jumpForceUp, ForceMode.Impulse); // this adds a upward force to the rb aka rigidbody, using an impulse which simulates a jump
            animator.SetTrigger("Jump"); //setting the OGani trigger
            Debug.Log("Jump Initiated");
            justJumped = true;
            jumpTimeCounter = Time.time;
        }

        if (transform.position.y < RespawnHeight) // if the transform is less than the respawn height than it will reset the postion then it will disable gravity and renable 
        {
            Debug.LogError("Player position: " + transform.position.y);
            Debug.LogError("RespawnHeight: " + RespawnHeight);

           //TO respawn the player
           RespawnOGPlayer();

           Debug.LogError("Player has repsawned back to the OG Position");
        }
    }
}
