using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.TimeZoneInfo;

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
    public static bool canMove;

    [Header("Crouch")] [SerializeField] private float crouchHeight;
    [SerializeField] private Vector3 crouchHeightPosition;
    private float originalHeight;
    private Vector3 originalCrouchHeightPosition;
    private bool isCrouching = false;

    [Header("Gravity")] [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float gravityMultiplier = 2;
    [SerializeField] private float groundedGravity = -0.5f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float jumpCooldown = 0.25f;
    private float velocityY;
    private float lastJumpTime;
    private bool isGrounded, isJumping, isFalling, isFreeFalling;

    [Header("Stamina")] [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 10f;
    private float currentStamina;
    public Image staminaSlider;

    [SerializeField] private UIManager uiManager;

    //tp
    [SerializeField] Transform gettingCaughtPoint;
    [SerializeField] Image imageTp;
    public bool againPatrol = false;

    [SerializeField] float health = 100;
    [SerializeField] bool isDie = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        camera = Camera.main.transform;
        currentSpeed = walkSpeed * Singleton.Instance.speedMultiplier;
        canMove = true;

        //CROUCH
        originalHeight = characterController.height;
        originalCrouchHeightPosition = characterController.center;

        //Remove cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //STAMINA
        currentStamina = maxStamina;
        UpdateStaminaUI();
    }

    void Update()
    {
        if (isDie == false)
        {
            HandleJump();
            Movement();

            if (direction.magnitude >= 0.9f && Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching && currentStamina > 1f)
            {
                currentSpeed = runSpeed * Singleton.Instance.speedMultiplier;
                animator.SetBool("isRun", true);
            }

            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.LeftShift) && direction.magnitude <= 0.5f || currentStamina < 1f)
            {
                currentSpeed = walkSpeed * Singleton.Instance.speedMultiplier;
                animator.SetBool("isRun", false);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ToggleCrouch();
            }

        }
        else if (isDie)
        {
            animator.SetBool("isDie", true);
        }

        //Stamina
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
            DrainStamina(staminaDrainRate * Time.deltaTime);
        else
            RefillStamina(staminaDrainRate * Time.deltaTime);

        UpdateStaminaUI();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && canMove)
        {
            //Rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Camera rotation with player
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Move - Speed
            characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime +
                                     Vector3.up * velocityY * Time.deltaTime);

            //Animator
            animator.SetBool("isWalk", true);
        }
        else
        {
            characterController.Move(Vector3.up * velocityY * Time.deltaTime);
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
            animator.SetBool("isCrouch", true);
            isCrouching = true;
        }
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            animator.SetBool("isJump", isJumping = false);
            animator.SetBool("isFall", isFalling = false);
            if (velocityY < 0f)
                velocityY = groundedGravity;
        }

        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space) && !isCrouching)
        {
            if (Time.time - lastJumpTime > jumpCooldown)
            {
                lastJumpTime = Time.time;
                velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
                animator.SetBool("isJump", isJumping = true);
            }
        }

        velocityY -= gravity * gravityMultiplier * Time.deltaTime;
        animator.SetBool("isGrounded", isGrounded = characterController.isGrounded);
        if (velocityY < 0 && isJumping)
        {
            animator.SetBool("isFall", isFalling = true);
        }
        else if (characterController.velocity.y <= -2f)
        {
            animator.SetBool("isFall", isFalling = true);
        }
    }

    //STAMINA
    private void DrainStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    private void RefillStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    private void UpdateStaminaUI()
    {
        staminaSlider.fillAmount = currentStamina / maxStamina;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportCollider"))
        {
            uiManager.SetActiveTransitionPanel();
        }
    }

    public IEnumerator gettingCaughtTeleport()
    {
        imageTp.DOColor(Color.black, 1.5f);


        yield return new WaitForSeconds(2f); // 2 saniye bekle

        imageTp.DOColor(new Color(0, 0, 0, 0), 1.5f);

        TeleportForEnemy();
        canMove = true;
    }

    private void TeleportForEnemy()
    {
        if (gettingCaughtPoint != null)
        {
            transform.position = gettingCaughtPoint.position;
            againPatrol = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TeleportCollider"))
        {
            TimePassWithButton.isHovering = false;
            uiManager.SetActiveTransitionPanel();
        }
    }

    public void dicreaseHealth(float damage) 
        {
        health -= damage;
        if (health <= 0)
        {
            isDie = true;
        }
        } 
}