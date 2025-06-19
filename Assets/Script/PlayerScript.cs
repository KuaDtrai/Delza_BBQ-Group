using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{   
    float movementSpeed = 6.0f; //speed
    float normalSpeed = 6.0f; // Normal movement speed
    float sprintSpeed = 10.0f; // Sprint speed when Shift is held
    float x, y, z;
    int Health = 3;
    Vector3 tempPos = new Vector3(0, 0, 0);
    Rigidbody2D rb; // Reference to the Rigidbody2D component
    //[SerializeField] float knockbackForce = 10f; // Adjustable knockback force in the Inspector
    //[SerializeField] float knockbackDuration = 0.5f; // Duration of the knockback effect

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {        
        // Check if Shift key is held down
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movementSpeed = sprintSpeed; // Increase speed when Shift is held
        }
        else
        {
            movementSpeed = normalSpeed; // Revert to normal speed when Shift is released
        }

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        tempPos = new Vector3(x, y);

        transform.Translate(tempPos * movementSpeed * Time.deltaTime);

        // Flip sprite on X-axis when moving left
        if (x < 0) // Moving left
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (x > 0) // Moving right
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (Health <= 0)
        {
            Destroy(this.gameObject);           
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("DIAMOND"))
        //{
        //    Destroy(collision.gameObject);
        //}
        //else if (collision.gameObject.CompareTag("TAKEDAMAGE"))
        //{
        //    Health--;
        //}
    }

}