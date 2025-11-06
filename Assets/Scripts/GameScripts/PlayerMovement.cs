using UnityEngine;
using UnityEngine.InputSystem;


//Пересування гравця
public class PlayerMovement : MonoBehaviour
{ 

    private float speed = 3f;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();      
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
        if (context.performed) speed = 5f;
        else if (context.canceled) speed = 3f;
    }
}
