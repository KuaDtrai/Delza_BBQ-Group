using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    public int coin;
    public int maxCoin;

    //public Sprite deathSnail;
    //public Sprite aliveSnail;
    public Image[] coins;

    public CoinHealth coinHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        coin = coinHealth.coin;
        maxCoin = coinHealth.maxCoin;

        for (int i = 0; i < coins.Length; i++)
        {
            //if(i<health)
            //{
            //    snails[i].sprite = aliveSnail;
            //}
            //else
            //{
            //    snails[i].sprite = deathSnail;
            //}
            if (i < coin)
            {
                coins[i].enabled = true;
            }
            else
            {
                coins[i].enabled = false;
            }
        }
    }
}
