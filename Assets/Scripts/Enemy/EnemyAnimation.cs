using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
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

    public void Die()
    {
        _anim.SetTrigger("death");
    }

}
