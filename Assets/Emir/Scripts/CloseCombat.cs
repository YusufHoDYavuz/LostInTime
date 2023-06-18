using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloseCombat : MonoBehaviour
{

    public float clickCooldown = 0.02f;

    
    public static int numberOfPunchs = 0;
    private float clickTimer = 0;

    public Animator animator;
    public static CloseCombat closeCombat;
    public static float range = 1f; 


    private void Start()
    {
        if (closeCombat == null)
            closeCombat = this;
    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.V) && !animator.GetBool("isWalk") &&!animator.GetBool("isRun") && animator.GetBool("isGrounded"))
            punchRequest();
        
    }

    // Update is called once per frame

    void punchRequest()
    {
        if (Time.time-clickTimer > clickCooldown && numberOfPunchs<2)
        {
            clickTimer = Time.time;
            animator.SetInteger("PunchCount", ++numberOfPunchs);
            PlayerController.canMove = false;
        }
    }

    public static void PunchCollision()
    {
        var ray = new Ray(closeCombat.transform.position, closeCombat.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, range))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("OUCH!! You hit the enemy.");
            }
        }
    }
    
}
