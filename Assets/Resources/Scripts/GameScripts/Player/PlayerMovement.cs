using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // ← додай

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    private float speed = 3f;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Sprint Settings")]
    public Slider staminaSlider; // ← перетягни слайдер з UI
    private float stamina = 10f;
    private float maxStamina = 10f;
    private bool isSprinting = false;

    [Header("Audio Settings")]
    public AudioSource footstepSource;
    public float footstepDelay = 0.4f;
    private float footstepTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
    }

    void Update()
    {
        HandleStamina();

        if (direction.sqrMagnitude > 0.01f)
            HandleFootsteps();
        else
        {
            if (footstepSource.isPlaying) footstepSource.Stop();
            footstepTimer = 0;
        }
    }

    void HandleStamina()
    {
        if (isSprinting && direction.sqrMagnitude > 0.01f)
        {
            stamina -= Time.deltaTime; // витрачається за 10 секунд
            if (stamina <= 0)
            {
                stamina = 0;
                speed = 3f; // примусово зупиняємо біг
            }
        }
        else
        {
            stamina += Time.deltaTime; // відновлюється за 10 секунд
            stamina = Mathf.Min(stamina, maxStamina);
        }

        if (staminaSlider != null)
            staminaSlider.value = stamina;
    }

    void HandleFootsteps()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0)
        {
            footstepSource.Play();
            float currentDelay = speed > 3f ? footstepDelay / 1.5f : footstepDelay;
            footstepTimer = currentDelay;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isMoving", true);
        if (context.canceled)
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat("lastHorizontal", direction.x);
            animator.SetFloat("lastVertical", direction.y);
        }
        direction = context.ReadValue<Vector2>();
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (stamina > 0) // дозволяємо біг тільки якщо є стаміна
            {
                isSprinting = true;
                speed = 5f;
            }
        }
        else if (context.canceled)
        {
            isSprinting = false;
            speed = 3f;
        }
    }
}