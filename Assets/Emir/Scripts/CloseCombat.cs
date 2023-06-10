using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombat : MonoBehaviour
{

    public float clickCooldown = 0.01f;

    
    private int numberOfPunchs = 0;
    private float clickTimer = 0;

    public Animator animator;
   

    // Update is called once per frame
    void Update()
    {
        if (numberOfPunchs != 0 && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Cross Punch") || animator.GetCurrentAnimatorStateInfo(0).IsName("Punching"))|| animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1) 
            animator.SetInteger("PunchCount",numberOfPunchs = 0);

        
        if (Input.GetKey(KeyCode.V))
            punchRequest();
        
    }

    void punchRequest()
    {
        if (Time.time-clickTimer > clickCooldown && numberOfPunchs<2)
        {

            clickTimer = Time.time;
            animator.SetInteger("PunchCount",++numberOfPunchs);
            
        }
    }
    
}
