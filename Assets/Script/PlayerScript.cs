using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    float movementSpeed = 6.0f;
    float normalSpeed = 6.0f;
    float sprintSpeed = 10.0f;
    float x, y, z;
    public int Health = 5;    
    Vector3 tempPos = new Vector3(0, 0, 0);
    Rigidbody2D rb;

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        tempPos = new Vector3(x, y);

        transform.Translate(tempPos * movementSpeed * Time.deltaTime);

        if (x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DIAMOND"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("TELEPORT"))
        {
            // Check if current scene is "Level 1"
            if (SceneManager.GetActiveScene().name == "Village")
            {
                SceneManager.LoadScene("Level 1");
            }
            else if (SceneManager.GetActiveScene().name == "Level 1")
                SceneManager.LoadScene("Level 2");
            else if (SceneManager.GetActiveScene().name == "Level 2")
                SceneManager.LoadScene("Level 3");
            else SceneManager.LoadScene("Village");
        }
    }
}