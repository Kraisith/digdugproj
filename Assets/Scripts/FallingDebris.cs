using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//currently forces player through the ground, but that's fine because the player will get killed and die anyway
//although, i maybe need to find a way to not just outright kill the player, but damage instead, but getting crushed by a rock means certain death right?
public class FallingDebris : MonoBehaviour
{

    public Player player;
    public Enemy enemy;
    void Awake()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        enemy = collision.gameObject.GetComponent<Enemy>();
        Collider2D[] selfColliders = GetComponents<Collider2D>();
        Debug.Log("collision with tag: " + collision.gameObject.tag);
        if ((player))
        {
            Debug.Log("kill player"); //figure this out at some point
            player.killPlayer();
            //Physics.IgnoreLayerCollision(6, 12);
            for (int i = 0; i < selfColliders.Length; i++) //idk need help
            {
                Debug.Log("collider " + i + " = " + selfColliders[i]);
                Physics2D.IgnoreCollision(collision.collider, selfColliders[i]);
            }

        }
        if ((enemy))
        {
            Debug.Log("kill enemy");
            enemy.killEnemy();
            //Physics.IgnoreLayerCollision(12, 13);
            for (int i = 0; i < selfColliders.Length; i++) //idk need help
            {
                Debug.Log("collider "+ i + " = "+ selfColliders[i]);
                Physics2D.IgnoreCollision(collision.collider, selfColliders[i]);
            }
        }
    }
}
