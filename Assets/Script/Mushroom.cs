using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 2;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float agroRange = 6f;

    [SerializeField]
    private float loseAgroRange = 8f;

    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    public Animator animator;

    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Luôn giữ xoay mặc định để tránh nghiêng
        transform.rotation = Quaternion.identity;

        if (player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 direction = (player.position - transform.position).normalized;

            // Logic bắt đầu / kết thúc đuổi
            if (!isChasing && distToPlayer <= agroRange)
            {
                isChasing = true;
            }
            else if (isChasing && distToPlayer >= loseAgroRange)
            {
                isChasing = false;
                rb.linearVelocity = Vector2.zero;
            }

            if (isChasing)
            {
                Chase(direction);
            }
            else
            {
                StopChase();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void Chase(Vector2 direction)
    {
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        animator.SetBool("isChasing", true);

        // Quay mặt đúng hướng (giả sử sprite gốc hướng phải)
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void StopChase()
    {
        animator.SetBool("isChasing", false);
        rb.linearVelocity = Vector2.zero;
    }
}
