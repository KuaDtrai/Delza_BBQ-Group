using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public CoinHealth coinHealth;
    public int heal = 1;
    public int progress = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.HealHealth(heal);
            coinHealth.CoinProgress(progress);
            Destroy(gameObject);
        }
    }
}
