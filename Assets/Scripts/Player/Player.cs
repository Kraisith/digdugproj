using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable, IEntity
{
    
    private bool replaying;
    private CommandProcessor commProcess;
    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public bool isHit = false;

    public int Score { get; set; }


    public UnityEvent OnJumpEvent;

    public GameObject bulletPrefab;
    public Transform firePoint;

    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float sprintSpeed;

    [SerializeField] private Transform groundCheckTr;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayers;
    //get handle to rigidbody
    private Rigidbody2D _rigid;
    // Start is called before the first frame update
    //variable grounded = false
    //jumpforce variable
    private string currentDirection;
    [SerializeField] private string lastMovedDirection;
    private Vector2 myVelocity;
    private Vector2 currentInputDirection;
    private bool onGround;
    private bool isSprinting;
    private int numColliders;
    private Collider2D[] colliders = new Collider2D[10];

    //handle to playerAnimation
    private PlayerAnimation _playerAnim;

    //handle to spriteRenderer
    private SpriteRenderer _playerSprite;

    //handle to boxcollider2d
    private BoxCollider2D _collider;

    [SerializeField] private Transform[] digCheckPointsDown;
    [SerializeField] private Transform[] digCheckPointsUp;
    [SerializeField] private Transform[] digCheckPointsRight;
    [SerializeField] private Transform[] digCheckPointsLeft;
    [SerializeField] private bool Alive;


    [SerializeField] private int playerHealth;

    private LevelManager lvlMnger;

    private void Awake()
    {
        commProcess = GetComponent<CommandProcessor>();
        replaying = false;
        Alive = true;
        sprintSpeed = runSpeed * 2.0f;
        //assign handle of rigidbody
        _rigid = GetComponent<Rigidbody2D>();
        onGround = false;

        //assign handle to playerAnimation
        _playerAnim = GetComponent<PlayerAnimation>();

        //assign handle to SpriteRenderer
        _playerSprite = GetComponentInChildren<SpriteRenderer>();

        //assign handle to BoxCollider2D
        _collider = GetComponent<BoxCollider2D>();

        Health = playerHealth;
        MaxHealth = playerHealth;

        Score = 0;

        lvlMnger = FindObjectOfType<LevelManager>();

        Scene scene = SceneManager.GetActiveScene(); //honestly i dont know another way of doing this, i know it shouldnt be in the player class...
        string nameOfScene = scene.name;
        if (nameOfScene == "Level1")
        {
            lvlMnger.setNumEnemies(4);
        } else if (nameOfScene == "Level2")
        {
            lvlMnger.setNumEnemies(6);
            Score = LoadCurrScore();
        }

    }

    public int LoadCurrScore()
    {
        int score = 0;
        if (File.Exists(Application.persistentDataPath + "/currScore.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/currScore.save", FileMode.Open);
            SaveGame saveGame = (SaveGame)bf.Deserialize(file);
            file.Close();
            score = saveGame.score;
        }
        return score;
    }

    public Transform[] GetDigCheckPoints(string dir)
    {
        if (dir == "down")
        {
            return digCheckPointsDown;
        }
        if (dir == "up")
        {
            return digCheckPointsUp;
        }
        if (dir == "right")
        {
            return digCheckPointsRight;
        }
        if (dir == "left")
        {
            //return digCheckPointsLeft; //because im now flipping the entire thing, left actually uses the flipped right dig points
            return digCheckPointsRight;
        }
        //if dir == "none"
        return null;

    }

    // Update is called once per frame
    void Update()
    {
        if (replaying)
        {
            commProcess.Do();
        }
        //horizontal input for left and right
        //float move = Input.GetAxisRaw("Horizontal");

        //SendMessage("getPlayerPosition", transform.position);

        if (Keyboard.current.leftShiftKey.wasReleasedThisFrame) //this seems very messy to me, not sure where to put it
        {
            isSprinting = false;
        }


        if (Alive)
        {
            Move(currentInputDirection);
        }



        /*
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, groundLayers.value);
        if (hitInfo.collider != null)
        {
            _grounded = true;
        }
        */
        //if space key && grounded == true
        //current velocity = new velocity (current x, jumpforce)
        //grounded = false

        //2D raycast from player position downwards to ground
        //if hitInfo != null
        //grounded = true

        //current velocity = new velocity (horizontal input, current velocity.y);
        //_rigid.velocity = new Vector2(move*runSpeed, _rigid.velocity.y);

    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        numColliders = Physics2D.OverlapCircleNonAlloc(groundCheckTr.position, groundCheckRadius, colliders, groundLayers);
        onGround = (numColliders > 0);
    }
    public void Move(Vector2 direction) //made public for replay purposes, but it doesnt work
    {
        /*
        if (currentDirection != "none")
        {
            if (((currentDirection == "right") || (currentDirection == "left")) && (direction.y != 0)) //if there is an input and the player is moving laterally, and the input is vertical
            {
                direction.x = 0;
            }
            else if (((currentDirection == "up") || (currentDirection == "down")) && (direction.x != 0)) //if there is an input and the player is moving vertically, and the input is lateral
            {
                direction.y = 0;
            }
        }
        */


        myVelocity = _rigid.velocity;
        myVelocity.x = direction.x * runSpeed;
        myVelocity.y = direction.y * runSpeed;



        if (isSprinting)
        {
            myVelocity.x = direction.x * sprintSpeed;
            myVelocity.y = direction.y * sprintSpeed;
        }

        if (direction.x > 0)
        {
            setDirection("right");
            setLastDirection("right");

        } else if (direction.x < 0)
        {
            setDirection("left");
            setLastDirection("left");
        }

        if (direction.y > 0)
        {
            setDirection("up");
            setLastDirection("up");
        }
        else if (direction.y < 0)
        {
            setDirection("down");
            setLastDirection("down");
        }

        if (direction.x == 0 && direction.y == 0)
        {
            setDirection("none");
        }


        _rigid.velocity = myVelocity;


        _playerAnim.Move(direction.x);
        Flip(direction);

    }

    void setDirection(string dir)
    {
        currentDirection = dir;
    }

    void setLastDirection(string dir)
    {
        lastMovedDirection = dir;
    }

    void Flip(Vector2 direction)
    {
        //if direction.x is greater than 0, face right
        //else if direction.x is less than 0, face left
        //changed how flip works, since i need the hitbox to flip as well
        if (direction.x > 0)
        {
            //_playerSprite.flipX = false;

            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            //_playerSprite.flipX = true;

            transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    private void PerformJump()
    {
        if (!onGround)
        {
            return;
        }
        myVelocity = _rigid.velocity;
        myVelocity.y = jumpSpeed;
        _rigid.velocity = myVelocity;
        //tell animator to Jump
        StartCoroutine(activateJumpAnim());
    }

    IEnumerator activateJumpAnim()
    {
        _playerAnim.Jump(true);
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitUntil(() => onGround == true);

        _playerAnim.Jump(false);
    }

    private void PerformAttack() //fire laser/bullet
    {
        /*
        if (!onGround)
        {
            return;
        }
        */
        if (myVelocity.x != 0)
        {
            return;
        }
        /* //this was for shooting a bullet, but my guy swings a sword so it's not what I want
        Debug.Log("shoot laser/bullet");
        Vector3 pos = firePoint.transform.position;
        Quaternion rot = firePoint.transform.rotation;
        
        if (getLastDirection() == "up")
        {
            Debug.Log(getLastDirection());
            pos.x = 0 + this.transform.position.x;
            pos.y = 2.1f + this.transform.position.y;
            rot = Quaternion.Euler(0, 0, 90);

        }
        if (getLastDirection() == "down")
        {
            Debug.Log(getLastDirection());
            pos.x = 0 + this.transform.position.x;
            pos.y = -0.1f + this.transform.position.y;
            rot = Quaternion.Euler(0, 0, -90);
        }
        if (getLastDirection() == "right")
        {
            Debug.Log(getLastDirection());
            pos.x = 0.7f + this.transform.position.x;
            pos.y = 0.9f + this.transform.position.y;
            rot = Quaternion.Euler(0, 0, 0);
        }
        if (getLastDirection() == "left")
        {
            Debug.Log(getLastDirection());
            pos.x = -0.8f + this.transform.position.x;
            pos.y = 0.9f + this.transform.position.y;
            rot = Quaternion.Euler(0, 0, 180);

        }
        GameObject bullet = Instantiate(bulletPrefab, pos, rot);
        Destroy(bullet, 0.5f);
        */
        _playerAnim.Attack();
        //cue attack animation
    }

    private void PerformSprint()
    {
        isSprinting = true;

    }

    public void OnJump(InputAction.CallbackContext input)
    {
        //jumping isnt a thing anymore, maybe in a different mode but i'll think about that if/when the time comes
        //if (input.started) PerformJump();
    }
    public void OnMove(InputAction.CallbackContext input) //this is giga scuffed man
    {
        if (!replaying)
        {
            Vector2 inputtedDir = input.ReadValue<Vector2>();

            if ((inputtedDir.x != 0) && (inputtedDir.y != 0)) //if the attempted input is in more than one direction
            {
                if ((getLastDirection() == "up") || (getLastDirection() == "down")) //if previously going in vertical direction
                {
                    inputtedDir.y = 0;
                    if (inputtedDir.x > 0) //if wanting to go right
                    {
                        inputtedDir.x = 1;
                    }
                    else //if wanting to go left
                    {
                        inputtedDir.x = -1;
                    }
                }
                else
                if ((getLastDirection() == "right") || (getLastDirection() == "left")) //if previously going in horizontal direction
                {
                    inputtedDir.x = 0;
                    if (inputtedDir.y > 0) //if wanting to go up
                    {
                        inputtedDir.y = 1;
                    }
                    else //if wanting to go down
                    {
                        inputtedDir.y = -1;
                    }
                }
            }

            currentInputDirection = inputtedDir;
            Debug.Log(currentInputDirection);
        }
    }

    //public void OnUp(InputAction.CallbackContext input)
    //{
    //    Vector2 direction = Vector2.zero;
    //    direction.x = 0;
    //    direction.y = 1;
    //    Debug.Log(direction);
    //    currentInputDirection = direction;
    //}

    //public void OnDown(InputAction.CallbackContext input)
    //{
    //    Vector2 direction = Vector2.zero;
    //    direction.x = 0;
    //    direction.y = -1;
    //    Debug.Log(direction);
    //    currentInputDirection = direction;
    //}

    //public void OnLeft(InputAction.CallbackContext input)
    //{
    //    Vector2 direction = Vector2.zero;
    //    direction.y = 0;
    //    direction.x = -1;
    //    Debug.Log(direction);
    //    currentInputDirection = direction;
    //}

    //public void OnRight(InputAction.CallbackContext input)
    //{
    //    Vector2 direction = Vector2.zero;
    //    direction.y = 0;
    //    direction.x = 1;
    //    Debug.Log(direction);
    //    currentInputDirection = direction;
    //}

    public void OnFire(InputAction.CallbackContext input)
    {

        if (input.started) PerformAttack();
    }

    public void OnSprint(InputAction.CallbackContext input)
    {
        if (input.started) PerformSprint();
    }

    public string getDirection()
    {
        return currentDirection;
    }

    public string getLastDirection()
    {
        return lastMovedDirection;
    }

    public void killPlayer()
    {
        Health = 0;
        Alive = false;
        Debug.Log("player killed");

        Vector2 collSize = _collider.size;
        Vector2 collOffset = _collider.offset;
        collSize.y = 0.01f; //wasn't sure how to disable collision, so this is a workaround
        collOffset.y = 0.1f;
        _collider.size = collSize;
        _collider.offset = collOffset;

        _playerAnim.Die();
        _rigid.gravityScale = 1; //makes player not float if he gets rolled midair
        //play death animation
        //lvlMnger.LoadLevel("LaunchMenu"); //go to main menu, just for testing
        lvlMnger.ResetScene(); //restart the level
    }


    public Vector3 getPosition()
    {
        return transform.position;
    }

    public void Damage()
    {
        Debug.Log("Player Damage()");
        Health--;
        subtractScore(100); //lose 100 points for getting hit
        isHit = true;
        if (Health < 1)
        {
            killPlayer();
        }
        else
        {
            if (Alive)
            {


                _playerAnim.GetAnimator().SetTrigger("hurt");
            }
        }
        StartCoroutine(ResetHit());
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.6f);
        isHit = false;
    }

    public void Heal(int toHeal)
    {
        int newHealth = Health + toHeal;
        if (newHealth > MaxHealth)
        {
            newHealth = MaxHealth; //can't heal above max hp
        }
        Health = newHealth;
    }

    public void addScore(int toAddScore)
    {
        Score += toAddScore;
    }

    public void subtractScore(int toSubScore)
    {
        Score -= toSubScore;
        if (Score<0) //score cant go below 0
        {
            Score = 0;
        }
    }

    public Vector2 getCurrentMovement() 
    {
        return currentInputDirection;
    }

    public void setReplaying(bool repl)
    {
        replaying = repl;
    }
}
