using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 200f; 
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Vector3 startPosition;
    public float respawnHeight = -10f;
    public float respawnOffset = 2f;

    private bool isJumping = false;
    private bool isFalling = false;
    private Rigidbody rb;
    private Vector3 movement;
    [SerializeField]
    private Animator animator;  

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();  
    }

    void Start()
    {
        IsGrounded();
    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalMovement, 0f, verticalMovement) * moveSpeed;

        if (horizontalMovement != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), Time.deltaTime * turnSpeed);
        }

        if (movement != Vector3.zero) {
            animator.SetBool("isRunning", true);
            Debug.Log("Running animation should be playing");  // Debug statement 1
        } else {
            animator.SetBool("isRunning", false);
            Debug.Log("Idle animation should be playing");  // Debug statement 2
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            isFalling = false;
            Debug.Log("Jump");
        }

        if (transform.position.y < respawnHeight)
        {
            Respawn();
        }

        if (rb.velocity.y < 0 && !isJumping)
        {
            isFalling = true;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isJumping = false;
        }
    }

    private void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = startPosition + Vector3.up * respawnOffset;
        rb.useGravity = true;
        isFalling = false;
    }

    private bool IsGrounded()
    {
        bool grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (grounded)
        {
            Debug.Log("Grounded");
            isJumping = false;
            isFalling = false;
        }
        return grounded;
    }
}
