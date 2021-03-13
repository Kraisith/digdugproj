using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBandit : Enemy, IDamageable
{
    public int Health { get; set; }

    public void Damage() //i want to know what enemy i'm hitting, so not merged into Enemy
    {
        Debug.Log("Damage");
        Health--;
        isHit = true;
        _enemyAnim.GetAnimator().SetBool("InCombat", true);
        if (Health < 1)
        {
            killEnemy();
        } else
        {
            _enemyAnim.GetAnimator().SetTrigger("hurt");
        }
        StartCoroutine(ResetHit());
    }

    public override void Init()
    {
        base.Init();
        Health = base.health;
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.6f);
        isHit = false;
    }
}
