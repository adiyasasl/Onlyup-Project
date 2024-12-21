using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float jumpPower = 7f;
    [SerializeField] float gravity = 10f;

    Vector3 moveDirection = Vector3.zero;

    [SerializeField] private bool canMove = true;

    [SerializeField] private Transform orientation;

    [SerializeField] private Animator animator;


    [SerializeField] CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        #region Handles Movement
        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;

        //PC CONTROLLER
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = canMove ? (isRunning ? runSpeed : walkSpeed) : 0;

        // Calculate the movement direction
        Vector3 movement = (forward * verticalInput) + (right * horizontalInput);

        // Normalize the movement vector to prevent faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Apply speed to the movement direction
        movement *= speed;

        float movementDirectionY = moveDirection.y;
        moveDirection = movement;

        #endregion
/*
        #region Handles Animation
        if (horizontalInput > 0 || horizontalInput < 0 || verticalInput > 0 || verticalInput < 0 && !isRunning)
        {
            animator.SetBool("isWalk", true);
            animator.SetInteger("CanRun", 1);
        }
        else if (horizontalInput > 0 || horizontalInput < 0 || verticalInput > 0 || verticalInput < 0 && isRunning)
        {
            animator.SetInteger("CanRun", 1);
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetInteger("CanRun", 0);
        }

        if (!isRunning)
        {
            animator.SetBool("isRun", false);
        }
        else
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isWalk", false);
            Debug.Log("Run");
        }
        #endregion
*/
        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        characterController.Move(moveDirection * Time.deltaTime);

    }
}
