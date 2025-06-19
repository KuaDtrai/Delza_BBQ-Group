using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 5;
    public GameObject object1;
    public GameObject object2;
    public bool isDead = false;

    public SpriteRenderer spriteRenderer;

    public GameController manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
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
            Death();            
        }
    }


    public void Death()
    {        
        if (isDead)
        {
            print("You lose");
           manager.GameOver();
        }       
    }

    public void HealHealth(int heal)
    {
        if (health < maxHealth)
        {
            health += heal;
        }
    }

}
