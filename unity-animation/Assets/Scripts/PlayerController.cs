using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 200f;  // Add a speed factor for turning the player
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        // Rotate the player in the direction of movement
        if (horizontalMovement != 0) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), Time.deltaTime * turnSpeed);
        }

        // Check for jump input
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

        // Execute the jump in FixedUpdate
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
