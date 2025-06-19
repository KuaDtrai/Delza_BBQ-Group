using UnityEngine;

public class CoinHealth : MonoBehaviour
{
    public int coin;
    public int maxCoin = 10;
    public GameObject portal;

    public GameController manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coin = maxCoin;
        portal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoinProgress(int progress)
    {
        coin -= progress;
        if(coin <= 0)
        {
            print("You win");
            portal.SetActive(true);
        }
    }
}
