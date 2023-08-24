using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Controls the behavior of the player character.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float jumpForce = 7f;
    private Rigidbody rb;

    [Header("Respawn Settings")]
    public Transform startPosition;
    public float respawnHeight = -10f;
    public float respawnOffset = 2f;
    private Quaternion originalRotation;

    [Header("Audio Clips")]
    public AudioClip GrassRun;
    public AudioClip StoneRun;
    [SerializeField] private AudioSource runningsoundsSFXAudioSource;
    public AudioClip FlatGrass;
    public AudioClip FlatStone;
    private AudioSource fallingFlatSFX;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;

    private Animator animator;

    private bool isFalling = false;
    private bool isJumpingRecently = false;
    private float jumpCooldown = 1.0f;
    private float timeSinceLastJump = 0.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
        runningsoundsSFXAudioSource = GameObject.Find("RunningSounds").GetComponent<AudioSource>();
        runningsoundsSFXAudioSource.loop = true;
        fallingFlatSFX = GameObject.Find("FallingFlatSounds").GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
        HandleAnimations();
        HandleJumping();
        HandleFalling();

        HandleRespawn();
    }

    private void HandleRotation()
    {
        // Handle player rotation based on input
        float horizontalRotation = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(0f, horizontalRotation * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation *= deltaRotation;
    }

    private void HandleMovement()
    {
        // Handle player movement based on input
        float verticalMovement = Input.GetAxis("Vertical");
        Vector3 movementDirection = transform.forward * verticalMovement;
        Vector3 movement = movementDirection * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    private void HandleAnimations()
    {
        // Handle player animations and sound based on movement
        Vector3 movementDirection = transform.forward * Input.GetAxis("Vertical");
        if (movementDirection != Vector3.zero && IsGrounded())
        {
            animator.SetBool("IsMoving", true);
            PlayFootstepSound();
        }
        else
        {
            animator.SetBool("IsMoving", false);
            StopFootstepSound();
        }
    }

    private void HandleJumping()
    {
        // Handle player jumping based on input and cooldown
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !isJumpingRecently)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("IsJumping", true);

            isJumpingRecently = true;
            timeSinceLastJump = 0.0f;
        }
        else
        {
            if (timeSinceLastJump < jumpCooldown)
            {
                animator.SetBool("IsJumping", false);
                timeSinceLastJump += Time.deltaTime;
            }
            else
            {
                isJumpingRecently = false;
            }
        }
    }

    private void HandleFalling()
    {
        // Handle player falling animations and states
        if (!IsGrounded() && !isFalling && !isJumpingRecently)
        {
            isFalling = true;
            animator.SetBool("IsFalling", true);
            Debug.Log("Falling");
        }
        else if ((IsGrounded() || isJumpingRecently) && isFalling)
        {
            isFalling = false;
            animator.SetBool("IsFalling", false);
            Debug.Log("Landed");
        }
    }

    private void HandleRespawn()
    {
        // Handle player respawn mechanics if fallen below respawn height
        if (transform.position.y < respawnHeight)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            transform.position = startPosition.position + Vector3.up * respawnOffset;
            rb.useGravity = true;
            transform.rotation = originalRotation;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
            animator.SetBool("FallingFlat", true);
            PerformSplatAnimation();
            PlayFallingFlatSound();
        }
    }

    private bool IsGrounded()
    {
        // Check if the player is grounded using a sphere cast
        return Physics.CheckSphere(groundCheck.position, 0.15f, ground);
    }

    private void PlayFootstepSound()
    {
        // Play footstep sound based on the ground material
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, ground))
        {
            string material = hit.collider.gameObject.tag;
            if (material != null)
            {
                Debug.Log("Player is standing on material: " + material);
            }
            runningsoundsSFXAudioSource.clip = (material == "Rock") ? StoneRun : GrassRun;
            if (!runningsoundsSFXAudioSource.isPlaying)
            {
                runningsoundsSFXAudioSource.Play();
            }
        }
    }

    private void StopFootstepSound()
    {
        // Stop the footstep sound if it's currently playing
        if (runningsoundsSFXAudioSource.isPlaying)
        {
            runningsoundsSFXAudioSource.Stop();
        }
    }

    private void PerformSplatAnimation()
    {
        // Trigger animation for the player being splatted on the ground
        animator.SetBool("GettingUp", true);
        animator.SetBool("BackToIdle", true);
    }

    private void PlayFallingFlatSound()
    {
        // Play the appropriate falling flat sound based on the ground material
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, ground))
        {
            string material = hit.collider.gameObject.tag;
            if (material != null)
            {
                Debug.Log("Player is splatted all over: " + material);
            }
            fallingFlatSFX.clip = (material == "Rock") ? FlatStone : FlatGrass;
            fallingFlatSFX.Play();
        }
    }
}
