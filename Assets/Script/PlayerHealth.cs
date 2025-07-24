using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 5;
    public GameObject object1;
    public GameObject object2;
    private bool isDead = false;

    public SpriteRenderer spriteRenderer;
    public Animator animator; // Thêm Animator để lấy trạng thái animation

    public GameController manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>(); // Lấy component Animator
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
        else if (collision.gameObject.CompareTag("ENEMY"))
        {
            // Lấy trạng thái animation hiện tại
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Kiểm tra nếu trạng thái là Attack1, Attack2 hoặc Attack3
            if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3"))
            {
                Destroy(collision.gameObject); // Hủy đối tượng ENEMY
            }
            // Kiểm tra nếu trạng thái là Block
            else if (stateInfo.IsName("Block") || stateInfo.IsName("Idle Block"))
            {
                // Bỏ qua, không làm gì cả
            }
            else
            {
                TakeDamage(1); // Gây sát thương nếu không phải Attack hoặc Block
            }
        }
    }

    public async void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            await Task.Delay(1);
            Destroy(object1);
            await Task.Delay(100);
        }
    }

    public Boolean Death(bool dead)
    {
        dead = false;
        if (isDead)
        {
            Instantiate(object2);
            dead = true;
            //manager.GameOver();
        }
        SceneManager.LoadScene("Village");
        return dead;
    }

    public void HealHealth(int heal)
    {
        if (health < maxHealth)
        {
            health += heal;
        }
    }
}