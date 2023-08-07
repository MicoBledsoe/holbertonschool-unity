using UnityEngine;
using System.Collections;

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
    private bool isGrounded = false;
    private bool isJumpKeyPressed = false;
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        StartCoroutine(CheckGroundedStatus());
    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        movement = new Vector3(horizontalMovement, 0f, verticalMovement) * moveSpeed;

        if (horizontalMovement != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), Time.deltaTime * turnSpeed);
        }

        if (movement != Vector3.zero)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (transform.position.y < respawnHeight)
        {
            Respawn();
        }

        if (!isGrounded && rb.velocity.y < -0.5f && !isJumping)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
            Debug.Log("Falling animation should be playing");
            Debug.Log("Started falling at time: " + Time.time);
        }
        else
        {
            isFalling = false;
            animator.SetBool("isFalling", false);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumpKeyPressed = true;
            animator.SetBool("isJumping", true);
            isFalling = false;
        }

        if (isGrounded && isFalling && Vector3.Distance(transform.position, startPosition) < 0.1f)
        {
            animator.SetBool("FallingToImpact", true);
        }
        else
        {
            animator.SetBool("FallingToImpact", false);
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (isJumpKeyPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isJumping = true; // Indicate that jump has started
            isJumpKeyPressed = false; // Reset the flag
        }
    }

    private void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = startPosition + Vector3.up * respawnOffset;
        rb.useGravity = true;

        isFalling = false;
        isJumping = false;
        isGrounded = true;
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isGrounded", true);
        Debug.Log($"Respawning. isFalling: {isFalling}, isJumping: {isJumping}, isGrounded: {isGrounded}");
        animator.Play("Falling Flat Impact");
    }

    private IEnumerator CheckGroundedStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
            if (isGrounded)
            {
                Debug.Log("Grounded");
                isJumping = false;
                isFalling = false;
                animator.SetBool("isJumping", false); // Reset isJumping to false in animator
                animator.SetBool("isFalling", false); // Reset isFalling to false in animator
            }
        }
    }
}
