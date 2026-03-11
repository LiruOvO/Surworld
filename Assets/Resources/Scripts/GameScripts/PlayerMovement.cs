using UnityEngine;
using UnityEngine.InputSystem;


//Пересування гравця
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    private float speed = 3f;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Audio Settings")]
    public AudioSource footstepSource; // Тут має бути AudioSource з призначеним Audio Random Container
    public float footstepDelay = 0.4f; // Затримка між кроками
    private float footstepTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            HandleFootsteps();
        }
        else
        {
            if (footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
            footstepTimer = 0;
        }
    }
    void HandleFootsteps()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            footstepSource.Play();
            // Розрахунок затримки (швидше, якщо біжимо)
            float currentDelay = speed > 3f ? footstepDelay / 1.5f : footstepDelay;
            footstepTimer = currentDelay;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    //Ходьба
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

    //Біг
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed) speed = 5f;
        else if (context.canceled) speed = 3f;
    }
}
