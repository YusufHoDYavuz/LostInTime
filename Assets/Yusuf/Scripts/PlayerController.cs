using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //COMPONENTS
    private CharacterController characterController;
    private Transform camera;
    private Animator animator;

    [Header("Movement")] [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSmoothTime;
    private float currentSpeed;
    private float turnSmoothVelocity;
    private Vector3 direction;

    [Header("Crouch")] [SerializeField] private float crouchHeight;
    [SerializeField] private Vector3 crouchHeightPosition;
    private float originalHeight;
    private Vector3 originalCrouchHeightPosition;
    private bool isCrouching = false;

    [Header("Gravity")]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float gravityMultiplier = 2;
    [SerializeField] float groundedGravity = -0.5f;
    [SerializeField] float jumpHeight = 3f;
    private float velocityY;
    
    

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        camera = Camera.main.transform;
        currentSpeed = walkSpeed;

        //CROUCH
        originalHeight = characterController.height;
        originalCrouchHeightPosition = characterController.center;

        //Remove cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleJump();
        Movement();
        if (direction.magnitude >= 0.9f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            animator.SetBool("isRun", true);
            animator.SetBool("isWalk", false);
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.LeftShift) && direction.magnitude <= 0.5f)
        {
            currentSpeed = walkSpeed;
            animator.SetBool("isRun", false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
        }

        if (direction.magnitude >= 0.1f && isCrouching)
        {
            animator.SetBool("isCrouchWalk", true);
            animator.SetBool("isCrouch", false);
        }
        else
        {
            animator.SetBool("isCrouchWalk", false);
        }

        if (direction.magnitude < 0.1f && isCrouching)
        {
            animator.SetBool("isCrouch", true);
        }

        
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //Rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Camera rotation with player
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Move - Speed
            characterController.Move(moveDirection.normalized*  currentSpeed * Time.deltaTime  + Vector3.up*velocityY*Time.deltaTime);

            //Animator
            animator.SetBool("isWalk", true);
        }
        else
        {
            characterController.Move( Vector3.up*velocityY*Time.deltaTime);
            animator.SetBool("isWalk", false);
        }
    }

    private void ToggleCrouch()
    {
        if (isCrouching)
        {
            // Not crouching
            characterController.height = originalHeight;
            characterController.center = originalCrouchHeightPosition;
            animator.SetBool("isCrouch", false);
            isCrouching = false;
        }
        else
        {
            // Crouching
            characterController.height = crouchHeight;
            characterController.center = crouchHeightPosition;
            animator.SetBool("isWalk", false);
            animator.SetBool("isCrouchWalk", true);
            animator.SetBool("isCrouch", true);
            isCrouching = true;
        }
    }

    private void HandleJump()
    {
        

        if (characterController.isGrounded)
        {
            animator.SetBool("isJump",false);
            if (velocityY < 0f)
                velocityY = groundedGravity;
           
           
        }
        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
            animator.SetBool("isJump",true);
           
        }
        velocityY -= gravity * gravityMultiplier * Time.deltaTime;
        animator.SetBool("isGrounded", characterController.isGrounded);
        
    }
}