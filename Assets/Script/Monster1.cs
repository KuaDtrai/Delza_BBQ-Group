using UnityEngine;

public class Monster1 : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;
    public int damage;

    private float distance;

    void Start()
    {
        
    }


    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        if (player.transform.position.x < 0) // Moving left
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.transform.position.x > 0) // Moving right
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (distance < distanceBetween) 
        { 
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }
}
