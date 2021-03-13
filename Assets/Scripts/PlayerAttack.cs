using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour //not actually player attack, just attack in general
{
    private bool _canDamage = true;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit = " + collision.name);

        IDamageable hit = collision.GetComponent<IDamageable>();
        if (hit != null) 
        {
            if (_canDamage)
            {
                hit.Damage();
                _canDamage = false;
                StartCoroutine(ResetDamage());
            }
        }
    }

    IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(0.5f);
        _canDamage = true;
    }
}
