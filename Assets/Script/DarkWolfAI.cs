using UnityEngine;

public class DarkWolfAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public enum AIState { Idle, Patrol, Chase, Attack, Damage, Death }
    private AIState currentState = AIState.Idle;

    [Header("Speeds & Ranges")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float attackRange = 1f;
    public float chaseRange = 5f;

    [Header("Patrol Settings")]
    public float patrolRadius = 5f;      // how far from home you’ll roam
    public float waitAtPoint = 1f;       // idle time at each patrol point

    private Vector2 homePosition;
    private Vector2 patrolPoint;
    private float pointReachedTime;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("References")]
    public Transform player;

    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        homePosition = transform.position;
        ChooseNewPatrolPoint();
        pointReachedTime = Time.time;
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle: HandleIdle(); break;
            case AIState.Patrol: HandlePatrol(); break;
            case AIState.Chase: HandleChase(); break;
            case AIState.Attack: HandleAttack(); break;
            case AIState.Damage: break;
            case AIState.Death: HandleDeath(); break;
        }
    }

    void HandleIdle()
    {
        animator.SetBool("IsWalking", false);
        rb.linearVelocity = Vector2.zero;

        // after waiting at point, go patrol again
        if (Time.time > pointReachedTime + waitAtPoint)
        {
            currentState = AIState.Patrol;
            ChooseNewPatrolPoint();
        }
    }

    void HandlePatrol()
    {
        animator.SetBool("IsWalking", true);

        Vector2 direction = (patrolPoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * patrolSpeed;
        spriteRenderer.flipX = direction.x > 0;

        // if we arrive
        if (Vector2.Distance(transform.position, patrolPoint) < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            pointReachedTime = Time.time;
            currentState = AIState.Idle;
        }
        // if player sneaks in range
        else if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            currentState = AIState.Chase;
        }
    }

    void ChooseNewPatrolPoint()
    {
        // pick a random point in a circle around homePosition
        Vector2 rand = Random.insideUnitCircle * patrolRadius;
        patrolPoint = homePosition + rand;
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
                player.GetComponent<PlayerHealth>()?.TakeDamage(1);
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
