using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    public Transform startPosition;
    public float respawnHeight = -10f;
    public float respawnOffset = 2f;
    private bool isFalling = false;
    private bool isJumpingRecently = false;
    private float jumpCooldown = 1.0f;
    private float timeSinceLastJump = 0.0f;
    private Quaternion originalRotation;
    private bool isRespawning = false;

    public AudioClip GrassRun;
    public AudioClip StoneRun;
    [SerializeField]
    private AudioSource runningsoundsSFXAudioSource;
    public AudioClip FlatGrass;
    public AudioClip FlatStone;
    private AudioSource fallingFlatSFX;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    private Animator animator;

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
        float horizontalRotation = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        
        // Quaternion-based rotation
        Quaternion deltaRotation = Quaternion.Euler(0f, horizontalRotation * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation *= deltaRotation;

        Vector3 movementDirection = transform.forward * verticalMovement;
        Vector3 movement = movementDirection * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (movementDirection != Vector3.zero && IsGrounded())
        {
            animator.SetBool("IsMoving", true);
            if (!runningsoundsSFXAudioSource.isPlaying)
            {
                PlaySound();
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
            if (runningsoundsSFXAudioSource.isPlaying)
            {
                runningsoundsSFXAudioSource.Stop();
            }
        }
        
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
            Splat();
            FlatSounds();
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, .15f, ground);
    }

    private void Splat()
    {
        animator.SetBool("GettingUp", true);
        animator.SetBool("BackToIdle", true);
    }

    private void PlaySound()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, ground))
        {
            string material = hit.collider.gameObject.tag;

            if (material != null)
            {
                Debug.Log("Player is standing on material: " + material);
            }

            if (hit.collider.gameObject.CompareTag("Rock"))
            {
                runningsoundsSFXAudioSource.clip = StoneRun;
                runningsoundsSFXAudioSource.Play();
            }
            else
            {
                runningsoundsSFXAudioSource.clip = GrassRun;
                runningsoundsSFXAudioSource.Play();
            }
        }
    }

    private void FlatSounds()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, ground))
        {
            string material = hit.collider.gameObject.tag;

            if (material != null)
            {
                Debug.Log("Player is splatted all over: " + material);
            }

            if (hit.collider.gameObject.CompareTag("Rock"))
            {
                fallingFlatSFX.clip = FlatStone;
                fallingFlatSFX.Play();
            }
            else
            {
                fallingFlatSFX.clip = FlatGrass;
                fallingFlatSFX.Play();
            }
        }
    }
}
