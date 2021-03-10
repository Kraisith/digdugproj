using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
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

    [SerializeField] private Transform[] digCheckPointsDown;
    [SerializeField] private Transform[] digCheckPointsUp;
    [SerializeField] private Transform[] digCheckPointsRight;
    [SerializeField] private Transform[] digCheckPointsLeft;
    [SerializeField] private bool Alive;

    private void Awake()
    {
        Alive = true;
        sprintSpeed = runSpeed * 2.0f;
        //assign handle of rigidbody
        _rigid = GetComponent<Rigidbody2D>();
        onGround = false;

        //assign handle to playerAnimation
        _playerAnim = GetComponent<PlayerAnimation>();

        //assign handle to SpriteRenderer
        _playerSprite = GetComponentInChildren<SpriteRenderer>();

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
            return digCheckPointsLeft;
        }
        //if dir == "none"
        return null;

    }

    // Update is called once per frame
    void Update()
    {
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
    private void Move(Vector2 direction)
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

        if (direction.x>0)
        {
            setDirection("right");
            setLastDirection("right");

        } else if (direction.x<0)
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
        if (direction.x > 0)
        {
            _playerSprite.flipX = false;
        }
        else if (direction.x < 0)
        {
            _playerSprite.flipX = true;
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
        yield return new WaitUntil(()=> onGround==true);

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
        if (myVelocity.x!=0)
        {
            return;
        }
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
        _playerAnim.Attack();
        //cue attack animation
    }

    private void PerformSprint()
    {
        isSprinting = true;
        
    }

    public void OnJump(InputAction.CallbackContext input)
    {
        if (input.started) PerformJump();
    }
    public void OnMove(InputAction.CallbackContext input)
    {
        currentInputDirection = input.ReadValue<Vector2>();
    }

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
        Alive = false;
        Debug.Log("player killed");
        _playerAnim.Die();
        _rigid.gravityScale = 1; //makes player not float if he gets rolled midair
        //play death animation

    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    




}
