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
    public float movementSmooth = 10f; // 🔥 smoothing factor

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

    float currentStamina;
    bool isGrounded;
    bool isSprinting;
    bool facingRight = true;

    Rigidbody2D rb;
    Animator animator;

    float moveInput;          // ⬅ disimpan untuk FixedUpdate
    Vector2 targetVelocity;   // ⬅ velocity tujuan

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

    // =========================
    // INPUT & LOGIC
    // =========================
    void Update()
    {
        CheckGround();

        if (!canMove)
        {
            StopFootstep();
            moveInput = 0f;
            animator.Play(isGrounded ? "Player_Idle" : "Player_Fall");
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        HandleSprint(moveInput);
        HandleJump();
        HandleFlip(moveInput);
        UpdateAnimations(moveInput);
        RegenerateStamina();
        UpdateStaminaUI();
        HandleFootstep(moveInput);
    }

    // =========================
    // PHYSICS (MULUS)
    // =========================
    void FixedUpdate()
    {
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        targetVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // 🔥 SMOOTH MOVEMENT
        rb.linearVelocity = Vector2.Lerp(
            rb.linearVelocity,
            targetVelocity,
            movementSmooth * Time.fixedDeltaTime
        );
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
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
        }
        else
        {
            isSprinting = false;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    void RegenerateStamina()
    {
        if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
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

    // ⚠️ ANIMASI TIDAK DIUBAH
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

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!value) StopFootstep();
    }
}
