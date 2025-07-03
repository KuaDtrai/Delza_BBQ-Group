using UnityEngine;

public class BringerOfDeathDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 2;

    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange = 6;

    [SerializeField]
    public float moveSpeed = 5;

    Rigidbody2D rb;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            // Calculate distance to player
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            Vector2 direction = player.position - transform.position;
            direction.Normalize();

            if (distToPlayer < agroRange)
            {
                Chase();
            }
            else
            {
                StopChase();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        animator.SetBool("isChasing", true);
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(-4, 4, 4);
        }
        else
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
    }

    public void StopChase()
    {
        animator.SetBool("isChasing", false);
        transform.position = Vector2.MoveTowards(transform.position, player.position, 0);
    }
}
