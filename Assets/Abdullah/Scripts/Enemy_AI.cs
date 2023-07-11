using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float distance = 10;
    [SerializeField] float moveSpeed = 2f;

    [SerializeField] targetForEnemy targetForEnemy;
    GameObject pointObject;
    Animator animator;

    [SerializeField] GameObject fireEffect;


    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletForce = 10f;
    [SerializeField] float destroyTime = 10f;
    [SerializeField] float fireInterval = 0.5f;

    [SerializeField] bool isFire = false;
    bool fireIntervalControl = true;


    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("functionForPointObject", 0f,10f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
      
        if (isFire && fireIntervalControl)
        {
            Fire();
        }
        
        Rotate();
       
        
    }
    private void Movement(Vector3 nextPosition)
    {
        nextPosition.y = gameObject.transform.position.y;
        if (Vector3.Distance(transform.position, Target.transform.position) < distance)
        {
            
            if (transform.position != nextPosition)
            {
                isFire = false;
                animator.SetBool("isWalk", true);
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

            }
        }
    }

    private void Rotate()
    {
        if (Vector3.Distance(transform.position, pointObject.transform.position) < 1f)
        {
            isFire = true;
            animator.SetBool("isWalk", false);
            // Hedefe dönme durumu
            Vector3 targetDirection = Target.position - transform.position;
            targetDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
    
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
            

        }       
        else if (Vector3.Distance(transform.position,pointObject.transform.position) > 1f)
        // noktaya bakarken
        {
            isFire = false;
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


    void Fire()
    {
        
        GameObject yeniMermi = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        GameObject yeniEfekt = Instantiate(fireEffect, firePoint.position, firePoint.rotation);
        Rigidbody mermiRigidbody = yeniMermi.GetComponent<Rigidbody>();
        mermiRigidbody.velocity =firePoint.forward * bulletForce;

        
        Destroy(yeniMermi, destroyTime);
        Destroy(yeniEfekt, 0.2f);
        StartCoroutine(fireStandby());

    }
    System.Collections.IEnumerator fireStandby()
    {
        fireIntervalControl= false;  // Ateþ yapýlamaz hale getirilir
        yield return new WaitForSeconds(fireInterval);  // Belirli bir süre beklenir
        fireIntervalControl = true;   // Ateþ yapýlabilir hale getirilir
    }


}
