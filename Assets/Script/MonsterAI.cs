using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    [Header("Movement")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Ranges")]
    public float chaseRange = 5f;
    public float attackRange = 1f;

    [Header("Attack")]
    public float attackCooldown = 1f;
    public float attackRadius = 0.5f;
    public int attackDamage = 1;
    public LayerMask playerLayer;

    [Header("Health & Shield")]
    public int maxHealth = 5;
    public float shieldDuration = 2f;
    public float shieldCooldown = 5f;

    Transform player;
    Animator animator;

    int currentPatrolIndex = 0;
    float lastAttackTime = -999f;
    int currentHealth;
    bool shieldAvailable = true;
    bool isShielding = false;

    enum State { Patrol, Chase, Attack, Shield, Dead }
    State state = State.Patrol;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (state == State.Dead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Patrol:
                Patrol();
                if (dist < chaseRange) state = State.Chase;
                break;

            case State.Chase:
                Chase();
                if (dist < attackRange) state = State.Attack;
                else if (dist > chaseRange) state = State.Patrol;
                break;

            case State.Attack:
                Attack();
                if (dist > attackRange) state = State.Chase;
                break;

            case State.Shield:
                // wait until shield ends
                break;
        }

        bool isMoving = (state == State.Patrol && patrolPoints.Length > 0)
                     || state == State.Chase;
        animator.SetBool("IsMoving", isMoving);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        Vector2 target = patrolPoints[currentPatrolIndex].position;
        MoveTowards(target, patrolSpeed);
        if (Vector2.Distance(transform.position, target) < 0.2f)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;
        string trigger = Random.value < 0.5f ? "Attack1" : "Attack2";
        animator.SetTrigger(trigger);
    }

    // Called by Attack1/Attack2 animation events
    void DealDamage()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (hit)
        {
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null) health.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        if (state == State.Dead || isShielding) return;
        animator.SetTrigger("TakeHit");
        currentHealth -= amount;

        if (currentHealth <= 0) Die();
        else if (shieldAvailable) StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        state = State.Shield;
        isShielding = true;
        shieldAvailable = false;
        animator.SetTrigger("Shield");

        yield return new WaitForSeconds(shieldDuration);

        isShielding = false;
        state = State.Patrol;

        yield return new WaitForSeconds(shieldCooldown);
        shieldAvailable = true;
    }

    void Die()
    {
        state = State.Dead;
        animator.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void MoveTowards(Vector2 target, float speed)
    {
        Vector2 pos = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position = pos;
        transform.localScale = new Vector3(
            target.x > transform.position.x ? 3 : -3,
            3, 3
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
