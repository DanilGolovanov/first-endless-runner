using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    public float moveSpeed;
    public float maxMoveSpeed = 15.1f;
    public float jumpForce;
    public float jumpTime;
    public float secondJumpTime;
    private float jumpTimeCounter;
    
    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public float groundCheckRadius;
    private bool grounded = false;
    private bool doubleJumpAllowed = false;
    
    public float speedMultiplier;
    public float speedIncreaseMilestone;
    private float speedMilestoneCount;

    public GameManager gameManager;
    private float moveSpeedStore;
    private float speedMilestoneCountStore;
    private float speedIncreaseMilestoneStore;

    public AudioSource jumpSound;
    public AudioSource deathSound;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
        speedMilestoneCount = speedIncreaseMilestone;
        //store initial values (for restarting the game)
        moveSpeedStore = moveSpeed;
        speedIncreaseMilestoneStore = speedIncreaseMilestone;
        speedMilestoneCountStore = speedMilestoneCount;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedUp();
        MoveForward();
        Jump();
        ChangeAnimations();
    }

    #endregion 

    #region Custom Methods

    void Jump()
    {
        //check if player is on the ground
        //grounded = Physics2D.IsTouchingLayers(_myCollider, whatIsGround);
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        //if player is on the ground allow double jump
        if (grounded)
        {
            doubleJumpAllowed = true;
        }

        //use "space" button to jump
        //jump if player is on the ground
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            SingleJump();
            jumpSound.Play();
        }
        //jump second time if player is in the air
        else if (doubleJumpAllowed && Input.GetKeyDown(KeyCode.Space))
        {
            SingleJump();
            jumpTimeCounter = secondJumpTime;
            doubleJumpAllowed = false;
            jumpSound.Play();
        }

        //float in the air if player holds "space"
        LongJump(grounded);
    }

    void LongJump(bool grounded)
    {
        //set the jump time counter
        if (grounded)
        {
            jumpTimeCounter = jumpTime;
        }
        //jump higher (and longer) if player holds space
        if (jumpTimeCounter > 0 && Input.GetKey(KeyCode.Space))
        {
            SingleJump();
            jumpTimeCounter -= Time.deltaTime;
        }
        //reset jump time counter if player doesn't hold space anymore
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimeCounter = 0;
        }
    }

    void SingleJump()
    {
        //move player up on y-axis
        _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, jumpForce);
    }

    void MoveForward()
    {
        //constantly move player forward
        _myRigidbody.velocity = new Vector2(moveSpeed, _myRigidbody.velocity.y);
    }

    void ChangeAnimations()
    {
        //set triggers for animations
        _myAnimator.SetFloat("Speed", _myRigidbody.velocity.x);
        _myAnimator.SetBool("Grounded", grounded);
    }

    void SpeedUp()
    {
        //if player passed the milestone increase the speed of the player and change some values
        if (transform.position.x > speedMilestoneCount)
        {
            //update milestone counter
            speedMilestoneCount += speedIncreaseMilestone;
            //increase distance for the next milestone 
            speedIncreaseMilestone *= speedMultiplier;
            //increase speed of the player until it reaches maxSpeed
            if (moveSpeed < maxMoveSpeed)
            {
                moveSpeed *= speedMultiplier;
            }           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Killbox")
        {
            gameManager.RestartGame();
            moveSpeed = moveSpeedStore;
            speedIncreaseMilestone = speedIncreaseMilestoneStore;
            speedMilestoneCount = speedMilestoneCountStore;
            deathSound.Play();
        }
    }

    #endregion
}
