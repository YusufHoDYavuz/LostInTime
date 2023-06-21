using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    public Transform[] waypoints;  
    public float moveSpeed = 5f; 

    public Transform target; 
    [SerializeField] float visionAngle = 60f;

    private int currentWaypointIndex = 0;  
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            walk();
        }

       // consola yazý yazdýrma !!!!!!!!!!!!!!!!!!!!!!!!!****!***'!**!*!*!*!*!*!*!*!*!**!
        if (target != null && IsTargetInVision(target))
        {
            Debug.Log("Hedef görüþ açýsýnda!");
        }
    }

    private void walk()
    {
        // Hedef noktaya doðru dönme
        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;
        targetDirection.y = 0f;  
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        animator.SetBool("isWalking", true);

        // Hedef noktaya doðru dönme (sadece yatay düzlemde)
        Vector3 targetPosition = new Vector3(waypoints[currentWaypointIndex].position.x, transform.position.y, waypoints[currentWaypointIndex].position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Hedef noktaya ulaþýldýðýnda bir sonraki hedefe gec
        if (transform.position == targetPosition)
        {
            animator.SetBool("isWalking", false);
            currentWaypointIndex++;
            Vector3 newPos = transform.position;
            newPos.y = 0f;
            transform.position = newPos;
        }

        

        

    }


    private bool IsTargetInVision(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        if (angle <= visionAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
 

