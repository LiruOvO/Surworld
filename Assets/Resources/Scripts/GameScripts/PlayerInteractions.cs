using NUnit.Framework.Constraints;
using UnityEngine;

//Анімація для добування ресурсів

public class PlayerInteractions : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 0.4f; // час паузи
    private float nextAttackTime = 0f;  // коли можна буде атакувати наступний раз

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
                    if (resource.CanInteractWith(selectedItem) && resource.shouldBeDestroyed)
                    {
                        animator.SetBool("isInteracting", true);
                        resource.ChangeResourceSprite(true);
                    }else if (!resource.shouldBeDestroyed)
                    {
                        resource.ChangeResourceSprite(false);
                    }
                }
            }
            if (hitCollider.tag == "NPC")
            {
                NPCDialogues dialogues = FindFirstObjectByType<NPCDialogues>();
                dialogues.TalkTo(hitCollider.name, this.gameObject);
            }

            if (hitCollider.tag == "Enemy")
            {
                if (Time.time >= nextAttackTime)
                {
                    EnemyAI enemyScript = hitCollider.GetComponent<EnemyAI>();

                        if (selectedItem == CollectableType.NONE)
                        {
                            enemyScript.TakeDamage(1);
                        }
                        else
                        {
                            Collectable itemData = ItemManager.Instance.GetItemByType(selectedItem);
                            if (itemData != null)
                            {
                                enemyScript.TakeDamage(itemData.damage);
                            }
                        }

                        animator.SetBool("isInteracting", true);
                        nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
        else
        {
            animator.SetBool("isInteracting", false);
        }
    }

    //Скидає стан в аніматорі
    public void OnAnimEnd()
    {
        animator.SetBool("isInteracting", false);
    }
}
