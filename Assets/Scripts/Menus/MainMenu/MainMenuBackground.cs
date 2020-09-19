using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.Menus
{
    public class MainMenuBackground : MonoBehaviour
    {
        #region Variables

        [Header("Speed")]
        [SerializeField, Tooltip("Speed with which player moves.")]
        private float moveSpeed;

        // rigid body attached to player
        private Rigidbody2D _myRigidbody;
        // animator to change animations
        private Animator _myAnimator;

        [Header("Ground")]
        [SerializeField, Tooltip("Layer(s) which represent ground.")]
        private LayerMask whatIsGround;
        [SerializeField, Tooltip("GameObject at the bottom of the player with attached collider to detect the ground.")]
        private Transform _groundCheck;
        [SerializeField, Tooltip("Radius of the groundCheck collider.")]
        private float groundCheckRadius;
        // check if player is on the ground
        private bool grounded = false;

        #endregion

        #region Default Methods
        private void Start()
        {
            // get corresponding values
            _myRigidbody = GetComponent<Rigidbody2D>();
            _myAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            MoveForward();
            ChangeAnimations();
        }

        #endregion

        #region Custom Methods
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
        }

        #endregion
    }
}
