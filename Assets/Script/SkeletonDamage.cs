using UnityEngine;

public class SkeletonDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 1;

    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    [SerializeField]
    public float moveSpeed;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(3, 3, 3);
        }
        else
        {
            transform.localScale = new Vector3(-3, 3, 3);
        }
    }

    public void StopChase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, 0);
    }
}