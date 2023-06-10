using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float turnSmoothTime;
    private float turnSmoothVelocity;
    
    private CharacterController characterController;
    
    private Transform camera;

    private Animator animator;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        camera = Camera.main.transform;
        Debug.Log(transform.GetChild(0).name);
        //Remove cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }   

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

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
            characterController.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
            
            //Animator
            animator.SetBool("isWalk",true);
        }
        else
        {
            animator.SetBool("isWalk",false);
        }
        
    }
}
