using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int givenScore = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //collect coin
            Player player = collision.GetComponent<Player>();
            if (player)
            {
                player.addScore(givenScore);
                Destroy(gameObject);
            }
        }
    }
}
