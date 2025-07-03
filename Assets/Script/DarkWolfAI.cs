using UnityEngine;

public class DarkWolfAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public enum AIState { Idle, Patrol, Chase, Attack, Damage, Death }
    private AIState currentState = AIState.Idle;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float attackRange = 1f;
    public float chaseRange = 5f;

    private Vector2 patrolPoint;
    public Transform player;

    public int maxHealth = 100;
    private int currentHealth;

    private float idleTime = 2f;
    private float startTime;

    private bool isAttacking = false;
    private Vector2 lastMoveDir = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        patrolPoint = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        startTime = Time.time;
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                HandleIdle();
                break;
            case AIState.Patrol:
                HandlePatrol();
                break;
            case AIState.Chase:
                HandleChase();
                break;
            case AIState.Attack:
                HandleAttack();
                break;
            case AIState.Damage:
                HandleDamage();
                break;
            case AIState.Death:
                HandleDeath();
                break;
        }
    }

    void HandleIdle()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);
        rb.linearVelocity = Vector2.zero;

        if (Time.time > startTime + idleTime)
        {
            currentState = AIState.Patrol;
            startTime = Time.time;
        }
    }

    void HandlePatrol()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);

        Vector2 direction = (patrolPoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * patrolSpeed;

        if (Vector2.Distance(transform.position, patrolPoint) < 0.1f)
        {
            patrolPoint = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }

        if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            currentState = AIState.Chase;
        }
    }

    void HandleChase()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttacking", false);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
        FacePlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange)
        {
            currentState = AIState.Attack;
        }
        else if (distance > chaseRange)
        {
            currentState = AIState.Patrol;
        }
    }

    void HandleAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", true);
        rb.linearVelocity = Vector2.zero;

        FacePlayer();
        Invoke(nameof(ResetAttack), 1f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.transform == player)
            {
                // player.GetComponent<PlayerHealth>()?.TakeDamage(10);
            }
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            currentState = AIState.Attack;
        }
        else
        {
            currentState = AIState.Chase;
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentState == AIState.Death) return;

        currentHealth -= amount;
        animator.SetTrigger("Damage");
        rb.linearVelocity = Vector2.zero;

        if (currentHealth <= 0)
        {
            currentState = AIState.Death;
        }
        else
        {
            currentState = AIState.Idle;
        }
    }

    void HandleDamage() { }

    void HandleDeath()
    {
        animator.SetBool("IsDead", true);
        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
    }

    void FacePlayer()
    {
        if (player != null)
        {
            float xDir = player.position.x - transform.position.x;
            if (xDir != 0)
                spriteRenderer.flipX = xDir > 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
