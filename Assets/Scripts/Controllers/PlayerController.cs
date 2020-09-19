using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [Header("Speed")]
        [SerializeField, Tooltip("Speed with which player moves.")]
        private float moveSpeed;
        [SerializeField, Tooltip("Maximum speed with which player can move.")]
        private float maxMoveSpeed = 15.1f;

        [SerializeField, Tooltip("Amount by which speed of the player will be multiplied when the milestone is reached.")]
        public float speedMultiplier;
        [SerializeField, Tooltip("Distance which player need to cover before applying speedMultiplier.")]
        private float speedIncreaseMilestone;
        // keep track of milestones
        private float speedMilestoneCount;

        // variables responsible for restart of the game
        private GameManager _gameManager;
        // store initial values, which are modified throughout the game, in order to use them to reset the game
        private float moveSpeedStore;
        private float speedMilestoneCountStore;
        private float speedIncreaseMilestoneStore;

        [Header("Jump")]
        [Tooltip("How high player can jump.")]
        public float jumpForce;
        [Tooltip("Maximum time of the first jump.")]
        public float jumpTime;
        [Tooltip("Maximum time of the second jump.")]
        public float secondJumpTime;
        // keep track of the jump time
        private float jumpTimeCounter;
        // check if player is allowed to make the second jump
        private bool doubleJumpAllowed = false;

        // rigid body attached to player
        private Rigidbody2D _myRigidbody;
        // animator to change animations
        private Animator _myAnimator;
        // indicator of when to apply double jump animation
        private bool doubleJumpAnimation;

        [Header("Ground")]
        [SerializeField, Tooltip("Layer(s) which represent ground.")]
        private LayerMask whatIsGround;
        [SerializeField, Tooltip("GameObject at the bottom of the player with attached collider to detect the ground.")]
        private Transform _groundCheck;
        [SerializeField, Tooltip("Radius of the groundCheck collider.")]
        private float groundCheckRadius;
        // check if player is on the ground
        private bool grounded = false;

        [Header("Sound")]
        [SerializeField, Tooltip("Audio file with jump sound.")]
        private AudioSource _jumpSound;
        [SerializeField, Tooltip("Audio file with death sound.")]
        private AudioSource _deathSound;

        #endregion

        #region Default Methods
        private void Start()
        {
            // get corresponding values
            _gameManager = FindObjectOfType<GameManager>();
            _myRigidbody = GetComponent<Rigidbody2D>();
            _myAnimator = GetComponent<Animator>();
            // initialize following variables
            jumpTimeCounter = jumpTime;
            speedMilestoneCount = speedIncreaseMilestone;
            // store initial values (for restart of the game)
            moveSpeedStore = moveSpeed;
            speedIncreaseMilestoneStore = speedIncreaseMilestone;
            speedMilestoneCountStore = speedMilestoneCount;
        }

        private void Update()
        {
            SpeedUp();
            MoveForward();
            Jump();
            ChangeAnimations();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // if player fell of the platform and touched the invisible box under the camera
            if (collision.gameObject.tag == "Killbox")
            {
                ResetGame();
            }
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Check if it's first jump or second jump and change all values related to the jump accordingly,
        /// keep track of the jump time and when the jump is allowed.
        /// </summary>
        private void Jump()
        {
            // check if player is on the ground
            grounded = Physics2D.OverlapCircle(_groundCheck.position, groundCheckRadius, whatIsGround);

            MakeFirstOrSecondJump(grounded);

            // finish double jump animation if player made double jump (after 1 second)
            Invoke("SwitchFromDoubleJumpAnimationToJump", 1);

            LongJump(grounded);
        }

        /// <summary>
        /// Make either first or second jump and change all related values accordingly.
        /// </summary>
        /// <param name="grounded">Variable which indicates if player is on the ground.</param>
        private void MakeFirstOrSecondJump(bool grounded)
        {
            // if player is on the ground allow double jump
            if (grounded)
            {
                doubleJumpAllowed = true;
            }

            // use "space" button to jump
            // jump if player is on the ground
            if (grounded && Input.GetKeyDown(KeyCode.Space))
            {
                SingleJump();
                _jumpSound.Play();
            }
            // jump second time if player is in the air and haven't made second jump already
            else if (doubleJumpAllowed && Input.GetKeyDown(KeyCode.Space))
            {
                DoubleJump();
            }
        }

        /// <summary>
        /// Make double jump and change all related values accordingly.
        /// </summary>
        private void DoubleJump()
        {
            SingleJump();
            jumpTimeCounter = secondJumpTime;
            doubleJumpAllowed = false;
            // if player makes double jump show different animation
            doubleJumpAnimation = true;
            _jumpSound.Play();
        }

        /// <summary>
        /// Push player up as long as he holds "space" and jumpTimeCounter is more than 0.
        /// </summary>
        /// <param name="grounded">Variable which indicates if player is on the ground.</param>
        private void LongJump(bool grounded)
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

        /// <summary>
        /// Push player up on y-axis using provided values.
        /// </summary>
        private void SingleJump()
        {
            //move player up on y-axis
            _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, jumpForce);
        }

        /// <summary>
        /// Move player forward at given speed.
        /// </summary>
        private void MoveForward()
        {
            //constantly move player forward
            _myRigidbody.velocity = new Vector2(moveSpeed, _myRigidbody.velocity.y);
        }

        private void ChangeAnimations()
        {
            //set triggers for animations
            _myAnimator.SetFloat("Speed", _myRigidbody.velocity.x);
            _myAnimator.SetBool("Grounded", grounded);
            _myAnimator.SetFloat("VerticalSpeed", _myRigidbody.velocity.y);
            _myAnimator.SetBool("DoubleJump", doubleJumpAnimation);
        }

        /// <summary>
        /// Deactiviate double jump animation.
        /// </summary>
        private void SwitchFromDoubleJumpAnimationToJump()
        {
            doubleJumpAnimation = false;
        }

        /// <summary>
        /// Change speed of the player when specified milestone is reached.
        /// </summary>
        private void SpeedUp()
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

        /// <summary>
        /// Reset all values that need to be reset in order to start game with the same settings.
        /// </summary>
        private void ResetGame()
        {
            _gameManager.FinishGame();
            moveSpeed = moveSpeedStore;
            speedIncreaseMilestone = speedIncreaseMilestoneStore;
            speedMilestoneCount = speedMilestoneCountStore;
            _deathSound.Play();
        }
        #endregion
    }
}
