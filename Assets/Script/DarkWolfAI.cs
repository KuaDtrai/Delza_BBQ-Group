using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
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
    public float patrolRadius = 5f;
    public float waitAtPoint = 1f;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("References")]
    public Transform player;

    private int currentHealth;
    private Vector2 homePosition;
    private Vector2 patrolPoint;
    private float pointReachedTime;
    private bool isAttacking;

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
        if (player == null) return;

        switch (currentState)
        {
            case AIState.Idle: HandleIdle(); break;
            case AIState.Patrol: HandlePatrol(); break;
            case AIState.Chase: HandleChase(); break;
            case AIState.Attack: HandleAttack(); break;
            case AIState.Death: HandleDeath(); break;
        }
    }

    void HandleIdle()
    {
        StopMovement();
        animator.SetBool("IsWalking", false);

        if (Time.time > pointReachedTime + waitAtPoint)
        {
            ChooseNewPatrolPoint();
            currentState = AIState.Patrol;
        }
    }

    void HandlePatrol()
    {
        MoveTo(patrolPoint, patrolSpeed, walk: true);

        if (Vector2.Distance(transform.position, patrolPoint) < 0.1f)
        {
            StopMovement();
            pointReachedTime = Time.time;
            currentState = AIState.Idle;
        }
        else if (IsPlayerInRange(chaseRange))
        {
            currentState = AIState.Chase;
        }
    }

    void HandleChase()
    {
        MoveTo(player.position, chaseSpeed, walk: false);

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist < attackRange) currentState = AIState.Attack;
        else if (dist > chaseRange) currentState = AIState.Patrol;
    }

    void HandleAttack()
    {
        if (isAttacking) return;

        StopMovement();
        isAttacking = true;

        animator.SetTrigger("Attack");
        animator.SetBool("IsAttacking", true);

        FacePlayer();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
            if (hit.transform == player)
                hit.GetComponent<PlayerHealth>()?.TakeDamage(1);

        Invoke(nameof(ResetAttack), 1f);
    }

    void ResetAttack()
    {
        isAttacking = false;
        currentState = IsPlayerInRange(attackRange) ? AIState.Attack : AIState.Chase;
    }

    public void TakeDamage(int amount)
    {
        if (currentState == AIState.Death) return;

        currentHealth -= amount;
        animator.SetTrigger("Damage");
        StopMovement();

        currentState = currentHealth <= 0 ? AIState.Death : AIState.Idle;
    }

    void HandleDeath()
    {
        animator.SetBool("IsDead", true);
        StopMovement();
        enabled = false;
    }

    void ChooseNewPatrolPoint()
    {
        patrolPoint = homePosition + Random.insideUnitCircle * patrolRadius;
    }

    void MoveTo(Vector2 target, float speed, bool walk)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        Vector2 next = (Vector2)transform.position + dir * speed * Time.deltaTime;
        rb.MovePosition(next);

        animator.SetBool("IsWalking", walk);
        animator.SetBool("IsRunning", !walk);
        spriteRenderer.flipX = dir.x > 0;
    }

    void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    bool IsPlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, player.position) < range;
    }

    void FacePlayer()
    {
        float dx = player.position.x - transform.position.x;
        if (dx != 0) spriteRenderer.flipX = dx > 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
