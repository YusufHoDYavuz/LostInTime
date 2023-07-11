using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float moveSpeed = 2f;

    [SerializeField] targetForEnemy targetForEnemy;
    GameObject pointObject;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("functionForPointObject", 0f,10f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
      
        
        Rotate();
        
    }
    private void Movement(Vector3 nextPosition)
    {
        if (transform.position != pointObject.transform.position)
        {
            animator.SetBool("isWalk", true);
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
       
        }
      
    }

    private void Rotate()
    {
        if (Vector3.Distance(transform.position, pointObject.transform.position) < 1f)
        {
            animator.SetBool("isWalk", false);
            // Hedefe dönme durumu
            Vector3 targetDirection = Target.position - transform.position;
            targetDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
    
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
            fire();

        }       
        else if (Vector3.Distance(transform.position,pointObject.transform.position) > 1f)
        // noktaya bakarken
        {
            Vector3 targetDirection = pointObject.transform.position - transform.position;
            targetDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            targetRotation.x = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
            Movement(pointObject.transform.position);

        }
    }

    private void functionForPointObject()
    {
        if (pointObject != null)
        {
            Destroy(pointObject);
        }
        targetForEnemy.GenerateRandomPoint();
        pointObject = targetForEnemy.pointObject;

    }


    static void fire()
    {
        
    }
}
