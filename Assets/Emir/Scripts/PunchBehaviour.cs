using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBehaviour : StateMachineBehaviour
{
    private bool canHit = true;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("PunchStyle", Random.Range(0,1f));
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canHit = true;
        PlayerController.canMove = true;
        if (stateInfo.IsName("Punch1") && CloseCombat.numberOfPunchs >= 2)
            PlayerController.canMove = false;
        else
        {
            PlayerController.canMove = true;
            animator.SetInteger("PunchCount", CloseCombat.numberOfPunchs = 0);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.8f)
        {
            if (!(stateInfo.IsName("Punch1") && CloseCombat.numberOfPunchs == 2))
                animator.SetInteger("PunchCount",CloseCombat.numberOfPunchs = 0);



        }
        else if (canHit && stateInfo.normalizedTime>= 0.4f)
        {
            canHit = false;
            CloseCombat.PunchCollision();
        }
    }
}
