using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected string powerUpDrop;
    protected bool Alive;
    protected bool Attacking = false;

    protected Player player;
    [SerializeField] protected Vector3 playerPosition;


    [SerializeField] protected int aggroDistance;

    protected SpriteRenderer _enemySprite;
    protected EnemyAnimation _enemyAnim;
    protected bool flip;
    protected Rigidbody2D _rigid;

    protected BoxCollider2D _collider;

    protected bool isHit = false;
    private LevelManager lvlMnger;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        Init();
    }

    private void Start()
    {
      
        
    }

    public virtual void Init() //initialise enemy subclass
    {
        Alive = true;
        _enemyAnim = GetComponentInChildren<EnemyAnimation>();
        _enemySprite = GetComponentInChildren<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        lvlMnger = FindObjectOfType<LevelManager>();
    }

    public virtual void Move()
    {
        if (Alive)
        {
            if (!Attacking)
            {



                if (isHit == false)
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
                        if ((Vector3.Distance(transform.position, playerPosition)) <= 1.5f) //if in attacking range
                        {
                            Attack();
                        }  else //if out of attacking range
                        {
                            _enemyAnim.GetAnimator().SetBool("InCombat", false);
                        }


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
                        _enemyAnim.GetAnimator().SetBool("InCombat", false);
                        _enemyAnim.Move(0);
                    }
                } 
                Flip();
                //else, _enemyAnim.Move(0)
            }
        }
    }

    public virtual void Flip()
    {

        if (flip) //if player is on right of enemy (because sprite starts out looking left) 
                  //changed how flip works, since i need the hitbox to flip as well
        {
            //_enemySprite.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            //_enemySprite.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }


    public virtual void Update()
    {
        playerPosition = player.getPosition();
        if (Alive)
        {
            Move();
        }
    }

    public virtual void killEnemy() //my enemies are all the same size, so the reassignment of size and offset can be the same for all enemies, otherwise it would have to be an abstract function
    {
        if (Alive) //workaround since this method gets called multiple times for some reason
        {
            player.addScore(1000); //player gets 1000 points for killing enemy 
            lvlMnger.decreaseEnemy();
        }
        Alive = false;
        Debug.Log("enemy killed");
        Vector2 collSize = _collider.size;
        Vector2 collOffset = _collider.offset;
        collSize.y = 0.01f; //wasn't sure how to disable collision, so this is a workaround
        collOffset.y = 0.1f;
        _collider.size = collSize;
        _collider.offset = collOffset;
        _enemyAnim.Die();
        _rigid.gravityScale = 1; //makes enemy not float if he gets rolled midair    
    }

    public virtual void Attack()
    {
        if (!Attacking)
        {
            Attacking = true;
            _enemyAnim.GetAnimator().SetBool("InCombat", true);
            _enemyAnim.GetAnimator().SetTrigger("attack");
            StartCoroutine(ResetAttack());
        }

    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.6f);
        Attacking = false;
    }
}
