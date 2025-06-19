using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int health;
    public int maxHealth;

    //public Sprite deathSnail;
    //public Sprite aliveSnail;
    public Image[] hero;

    public PlayerHealth playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hero.Length; i++)
        {
            //if(i<health)
            //{
            //    snails[i].sprite = aliveSnail;
            //}
            //else
            //{
            //    snails[i].sprite = deathSnail;
            //}
            if (i < health)
            {
                hero[i].enabled = true;
            }
            else
            {
                hero[i].enabled = false;
            }
        }
    }
}
