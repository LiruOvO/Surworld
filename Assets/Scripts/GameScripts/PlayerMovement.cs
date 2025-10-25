using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ //ѕересуванн€ гравц€

    private float speed = 2;
    private Vector3 directon;

    public Animator animator;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        directon = new Vector3(horizontal, vertical);

        AnimateMovement(directon);       
    }

    private void FixedUpdate()
    {
        this.transform.position += directon * speed * Time.deltaTime;
    }



    //ѕередача значень дл€ ан≥матора
    void AnimateMovement(Vector3 direction)
    {
        if(direction.magnitude > 0)//√равець пересуваЇтьс€
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
        }
        else//гравець не пересуваЇтьс€
        {
            animator.SetBool("isMoving", false);
        }
    }


}
