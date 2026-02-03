using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float sprintSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;

    [Header("UI Settings")]
    public Slider staminaBar;
    public Image staminaFill;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip footstepSound;
    public AudioClip jumpSound;

    private float currentStamina;
    private bool isGrounded;
    private bool isSprinting;
    private bool facingRight = true;

    private Rigidbody2D rb;
    private Animator animator;

    // Dipakai oleh DialogManager
    public bool canMove = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.clip = footstepSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }
    }

    void Update()
    {
        CheckGround();

        if (!canMove)
        {
            StopFootstep();
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            animator.Play(isGrounded ? "Player_Idle" : "Player_Fall");
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        HandleSprint(moveInput);
        MovePlayer(moveInput);
        HandleJump();
        HandleFlip(moveInput);
        UpdateAnimations(moveInput);
        RegenerateStamina();
        UpdateStaminaUI();
        HandleFootstep(moveInput);
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void MovePlayer(float moveInput)
    {
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            StopFootstep();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            PlayJumpSound();
        }
    }

    void HandleSprint(float moveInput)
    {
        bool sprintHeld = Input.GetKey(KeyCode.LeftShift);

        if (sprintHeld && currentStamina > 0f && Mathf.Abs(moveInput) > 0.1f)
        {
            isSprinting = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
        }
        else
        {
            isSprinting = false;
        }
    }

    void RegenerateStamina()
    {
        if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;

            if (staminaFill != null)
                staminaFill.color = Color.Lerp(
                    Color.red,
                    Color.green,
                    currentStamina / maxStamina
                );
        }
    }

    void HandleFlip(float moveInput)
    {
        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }

    void UpdateAnimations(float moveInput)
    {
        if (isGrounded)
        {
            animator.Play(Mathf.Abs(moveInput) < 0.1f ? "Player_Idle" : "Player_Run");
        }
        else
        {
            animator.Play(rb.linearVelocity.y > 0.1f ? "Player_Jump" : "Player_Fall");
        }
    }

    void HandleFootstep(float moveInput)
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;

        if (isGrounded && isMoving)
        {
            if (!audioSource.isPlaying && footstepSound != null)
                audioSource.Play();
        }
        else
        {
            StopFootstep();
        }
    }

    void StopFootstep()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    void PlayJumpSound()
    {
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }

    // Dipanggil DialogManager
    public void SetCanMove(bool value)
    {
        canMove = value;

        if (!value)
            StopFootstep();
    }
}