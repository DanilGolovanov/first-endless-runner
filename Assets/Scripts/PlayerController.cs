using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    public float moveSpeed;
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;
    public LayerMask whatIsGround;

    private Rigidbody2D _myRigidbody;
    private Collider2D _myCollider;
    private Animator _myAnimator;

    private bool grounded = false;
    private bool doubleJumpAllowed = false;

    #endregion

    #region Default Methods

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myCollider = GetComponent<Collider2D>();
        _myAnimator = GetComponent<Animator>();
        jumpTimeCounter = jumpTime;
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        Jump();
        ChangeAnimations();
    }

    #endregion 

    #region Custom Methods

    void Jump()
    {
        //check if player is on the ground
        grounded = Physics2D.IsTouchingLayers(_myCollider, whatIsGround);

        //if player is on the ground allow double jump
        if (grounded)
        {
            doubleJumpAllowed = true;
            jumpTimeCounter = jumpTime;
        }

        //use "space" button to jump
        //jump if player is on the ground
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            SingleJump();
        }
        //jump second time if player is in the air
        else if (doubleJumpAllowed && Input.GetKeyDown(KeyCode.Space))
        {
            SingleJump();
            doubleJumpAllowed = false;
        }

        //jump higher if player holds space
        if (jumpTimeCounter > 0 && Input.GetKey(KeyCode.Space))
        {
            SingleJump();
            jumpTimeCounter -= Time.deltaTime;
            Debug.Log(jumpTimeCounter);
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
        _myAnimator.SetFloat("Speed", _myRigidbody.velocity.x);
        _myAnimator.SetBool("Grounded", grounded);
    }

    #endregion
}
