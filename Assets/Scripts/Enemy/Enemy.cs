using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected string powerUpDrop;
    protected bool Alive;


    [SerializeField] protected GameObject player;
    [SerializeField] protected Vector3 playerPosition;


    [SerializeField] protected int aggroDistance;

    protected SpriteRenderer _enemySprite;
    protected EnemyAnimation _enemyAnim;
    protected bool flip;


    private void Start()
    {
        Init(); 
        
    }

    public virtual void Init() //initialise enemy subclass
    {
        Alive = true;
        _enemyAnim = GetComponentInChildren<EnemyAnimation>();
        _enemySprite = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Move()
    {
        //get x and y differences between this and player
        //whichever one is larger, move in that direction (so it doesn't move directly diagonal
        //needs to be a way to handle if the distances are exactly equal, if so favour x over y movement

        //if moving
        Vector3 posDif = transform.position - playerPosition;
        if (playerPosition.x > transform.position.x) //if player is on right of enemy
        {
            flip = true;
        }
        else
        {
            flip = false;
        }

        if ((Vector3.Distance(transform.position, playerPosition)) <= aggroDistance)
        {
            if ((Mathf.Abs(transform.position.x - playerPosition.x)) >= Mathf.Abs((transform.position.y - playerPosition.y))) // if the distance between the x components is larger than the y components
            {
                posDif.y = transform.position.y; //basically, moving in a straight line horizontally
                posDif.x = playerPosition.x;
                transform.position = Vector3.MoveTowards(transform.position, posDif, speed * Time.deltaTime);
                _enemyAnim.Move(0.2f);
            }
            else //if the distance between the y components is larger than the x components
            {
                posDif.x = transform.position.x; //basically, moving in a straight line vertically
                posDif.y = playerPosition.y;
                transform.position = Vector3.MoveTowards(transform.position, posDif, speed * Time.deltaTime);
                _enemyAnim.Move(0.2f);
            }



            //transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
            //_enemyAnim.Move(0.2f);
        }
        else //meaning, out of range
        {
            _enemyAnim.Move(0);
        }

        Flip();
        //else, _enemyAnim.Move(0)
    }

    public virtual void Flip()
    {

        if (flip) //if player is on right of enemy (because sprite starts out looking left)
        {
            _enemySprite.flipX = true;
        }
        else
        {
            _enemySprite.flipX = false;
        }

    }


    public virtual void Update()
    {
        playerPosition = player.GetComponent<Player>().getPosition();
        if (Alive)
        {
            Move();
        }
    }

    public void killEnemy()
    {
        Alive = false;
        Debug.Log("enemy killed");
        _enemyAnim.Die();
    }
}
