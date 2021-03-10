using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float bulletDamage = 10f;

    Rigidbody2D rb2d;

    //up shot=0,2.1, rotation 90 on z
    //down shot=0,-0.1, rotation -90 on z
    //right shot=0.7,0.9, rotation 0 on z
    //left shot=-0.8,0.9, rotation 180 on z
    private void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
        Debug.Log(rb2d.gameObject);
        Vector2 force = transform.right * bulletSpeed;
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreLayerCollision(6, 9);
            return;
        }
        Destroy(gameObject);
    }
}
