using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeart : MonoBehaviour
{

    public int restoreHealth = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //collect heart
            Player player = collision.GetComponent<Player>();

            if (player)
            {
                player.Heal(restoreHealth);
                Destroy(gameObject);
            }
        }
    }
}
