using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //handle to animator
    private Animator _anim;

    // Start is called before the first frame update
    void Awake()
    {
        //assign handle to animator
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //transition between idle and move
    public void Move(float move)
    {
        //anim set float Move, move
        _anim.SetFloat("Move", Mathf.Abs(move));
    }

    //transition between idle and jump
    public void Jump(bool jumping)
    {
        _anim.SetBool("Jumping", jumping);
        
    }

    //attack method
    public void Attack()
    { /*
        if (_anim.GetBool("attack3") == true)
        {
            _anim.ResetTrigger("attack1");
            _anim.ResetTrigger("attack2");
            _anim.ResetTrigger("attack3");

            _anim.SetTrigger("attack1");
        }
        else if (_anim.GetBool("attack2") == true)
        {
            _anim.ResetTrigger("attack1");
            _anim.ResetTrigger("attack2");
            _anim.ResetTrigger("attack3");

            _anim.SetTrigger("attack3");
        }
        else if (_anim.GetBool("attack1")== true)
        {
            _anim.ResetTrigger("attack1");
            _anim.ResetTrigger("attack2");
            _anim.ResetTrigger("attack3");
            _anim.SetTrigger("attack2");
        } else
        {
            _anim.SetTrigger("attack1");
        } */
        _anim.SetTrigger("attack1");
    }
    
    //death animation
    public void Die()
    {
        _anim.SetTrigger("death");
    }

}
