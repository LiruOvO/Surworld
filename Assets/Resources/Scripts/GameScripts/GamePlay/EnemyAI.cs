using UnityEngine;
using TMPro;


// Скрипт поведінки для ворогів

public class EnemyAI : MonoBehaviour
{

    public float health = 20;
    public float attack = 5;
    [Header("Radius Settings")]
    public float patrolRadius = 5f; // Тепер це і зона патруля, і зона агру
    public float maxChaseDistance = 10f;



    [Header("UI")]
    public GameObject hpUI; 
    public TextMeshProUGUI hpText;

    [Header("Speed Settings")]
    public float patrolSpeed;
    public float chaseSpeed;

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f; // раз у скільки секунд б'є
    private float attackTimer = 0f;


    [Header("Combat Settings")]
    public float attackDistance = 1.2f;
    public float stopDistance = 0.3f;
    private Collider2D enemyCollider;
    private Collider2D playerCollider;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 patrolTarget;

    private enum State { Patrol, Chase, Return }
    private State currentState;

    // Для періодичних зупинок при патрулюванні
    private Animator anim;
    private bool isWaiting = false;
    private float waitTime = 0f;
    private float minWait = 2f;
    private float maxWait = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        patrolTarget = startPosition;
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentState = State.Patrol;
        ChooseNewPatrolPoint();

        enemyCollider = GetComponent<Collider2D>();

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCollider = playerObj.GetComponent<Collider2D>();
        }
    }

    void Update()
    {
        if (player == null) return;
        attackTimer -= Time.deltaTime;

        // Відстань від гравця до ТОЧКИ СПАВНУ слайма
        float distancePlayerToSpawn = Vector2.Distance(startPosition, player.position);

        if (distancePlayerToSpawn <= maxChaseDistance)
        {
            hpUI.SetActive(true);
        }
        else
        {
            hpUI.SetActive(false);
        }

        switch (currentState)
        {
            case State.Patrol:
                HandlePatrol();

                // АГР: якщо гравець зайшов у радіус патрулювання (відстань від спавну)
                if (distancePlayerToSpawn <= patrolRadius)
                    currentState = State.Chase;
                break;

            case State.Chase:
                MoveTo(player.position, chaseSpeed);
                anim.SetBool("IsAggroMove", true);

                // ПОВЕРНЕННЯ: якщо гравець вибіг за другу, велику зону
                if (distancePlayerToSpawn > maxChaseDistance)
                    currentState = State.Return;
                break;

            case State.Return:
                MoveTo(startPosition, patrolSpeed, true);
                anim.SetBool("IsAggroMove", false);

                if (Vector2.Distance(transform.position, startPosition) < stopDistance)
                {
                    currentState = State.Patrol;
                    ChooseNewPatrolPoint();
                }

                // ПОВТОРНИЙ АГР: якщо поки ми йшли додому, гравець знову вліз у патруль-зону
                if (distancePlayerToSpawn <= patrolRadius)
                    currentState = State.Chase;
                break;
        }
    }

    void HandlePatrol()
    {
        if (!isWaiting)
        {
            MoveTo(patrolTarget, patrolSpeed, true);
            if (Vector2.Distance(transform.position, patrolTarget) < stopDistance)
            {
                isWaiting = true;
                waitTime = Random.Range(minWait, maxWait);
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("IsMoving", false);
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                isWaiting = false;
                ChooseNewPatrolPoint();
            }
        }
        anim.SetBool("IsAggroMove", false);
    }

    void MoveTo(Vector2 target, float speed, bool ignoreAttackDistance = false)
    {
        float realDistance = Vector2.Distance(
            enemyCollider.ClosestPoint(player.position),
            playerCollider.ClosestPoint(transform.position)
        );

        // якщо близько → атакуємо
        if (!ignoreAttackDistance && realDistance <= attackDistance)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("IsMoving", false);

            TryAttackPlayer();
            return;
        }
        else
        {
            // ← ВАЖЛИВО: якщо гравець відійшов — скидаємо атаку
            anim.ResetTrigger("isAttacking");
        }

        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();

        rb.linearVelocity = direction * speed;

        anim.SetFloat("MoveX", direction.x);
        anim.SetFloat("MoveY", direction.y);
        anim.SetBool("IsMoving", true);
    }

    void ChooseNewPatrolPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        patrolTarget = startPosition + randomPoint;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) startPosition = transform.position;

        // Малюємо зону патруля та агру (синя)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);

        // Малюємо зону, де він здається (червона)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPosition, maxChaseDistance);
    }


    public void TakeDamage(int amount)
    {
        if (health <= 0) return;
        health -= amount;
        hpText.text = Mathf.Max(0, health).ToString();
        if (health <= 0) Die();
    }
    private void Die()
    {
        anim.SetBool("isDead", true);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = false;

        Collectable itemPrefab = ItemManager.Instance.GetItemByType(CollectableType.SLIME);
        Vector3 spawnLocation = transform.position;
        Vector3 spawnOffset = new Vector3(Random.Range(-0.3f, 0f), Random.Range(-1.3f, -1.5f), 0f);
        Transform collectablesParent = GameObject.Find("Collectables")?.transform;
        Instantiate(itemPrefab, spawnLocation + spawnOffset, Quaternion.identity, collectablesParent);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2.0f);        
    }
    void TryAttackPlayer()
    {
        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;

        // беремо скрипт гравця
        HealthManager playerHealth = player.GetComponent<HealthManager>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)attack);
        }

        anim.SetTrigger("isAttacking"); // якщо буде анімація удару
    }

}