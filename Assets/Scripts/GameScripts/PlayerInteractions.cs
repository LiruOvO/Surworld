using NUnit.Framework.Constraints;
using UnityEditor.Tilemaps;
using UnityEngine;

//Анімація для добування ресурсів

public class PlayerInteractions : MonoBehaviour
{
    private bool isMining = false;
    private float maxDistance = 1.5f;
    private Vector2 interactionOffset = new Vector2(0f, -0.6f);
    private Animator animator;
    public CollectableType selectedItem = CollectableType.NONE;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInterction();
        }
    }

    private void HandleInterction()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Отримання позиції миші
        Collider2D hitCollider = Physics2D.OverlapPoint(clickPosition); //Створення променю від миші
        if (hitCollider != null)
        {
            Resource resource = hitCollider.GetComponent<Resource>(); //Перевірка що це за ресурс
            Debug.Log(hitCollider.name);
            if (resource != null)
            {
                Vector2 playerCheckPosition = (Vector2)transform.position + interactionOffset;
                float distance = Vector2.Distance(playerCheckPosition, hitCollider.transform.position); //Розріхунок дистанції між гравцем і ресурсом

                if (distance <= maxDistance)
                {
                    if (resource.type == Resource.ResourceType.Stone && selectedItem == CollectableType.PICKAXE) //Для каменю
                    {
                        isMining = true;
                        animator.SetBool("isMining", true);
                        resource.ChangeResourceSprite();
                    }
                }
            }
        }
        else
        {
            isMining = false;
            animator.SetBool("isMining", false);
        }
    }

    //Скидає стан в аніматорі
    public void OnAnimEnd()
    {
        isMining = false;
        animator.SetBool("isMining", false);
    }
}
