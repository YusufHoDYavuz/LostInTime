using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] private Transform Target;
    public float distanceEnemy = 10;
    [SerializeField] private float moveSpeed = 2f;

   
    GameObject pointObject;
    Animator animator;

    [SerializeField] GameObject fireEffect;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float destroyTime = 10f;
    [SerializeField] private float fireInterval = 0.5f;

    [SerializeField] private bool isFire;
    bool fireIntervalControl = true;

    [SerializeField] int health = 100;
    bool isDie;

    bool isFirst = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InvokeRepeating("PointObject", 5f, 10f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        if (isFire && fireIntervalControl)
            Fire();

        Rotate();

        if (Vector3.Distance(transform.position, Target.position) <= distanceEnemy)
        {
            if (isFirst)
            {
                
                isFirst = false;
            }
        }
    }

    private void Movement(Vector3 nextPosition)
    {
        if (isDie == false)
        {
            nextPosition.y = gameObject.transform.position.y;
            if (Vector3.Distance(transform.position, Target.transform.position) < distanceEnemy)
            {
                if (transform.position != nextPosition)
                {
                    isFire = false;
                    animator.SetBool("isWalk", true);
                    transform.position =
                    Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void Rotate()
    {
        if (!isDie)
        {
            if (pointObject != null)
            {
                if (Vector3.Distance(transform.position, pointObject.transform.position) < 1f)
                {
                    isFire = true;
                    animator.SetBool("isWalk", false);

                    Vector3 targetDirection = Target.position - transform.position;
                    targetDirection.y = 0f;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
                }
                else if (Vector3.Distance(transform.position, pointObject.transform.position) > 1f)
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
        }
    }

 


    void Fire()
    {
        if (!isDie)
        {
            GameObject yeniMermi = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            GameObject yeniEfekt = Instantiate(fireEffect, firePoint.position, firePoint.rotation);
            Rigidbody mermiRigidbody = yeniMermi.GetComponent<Rigidbody>();
            mermiRigidbody.velocity = firePoint.forward * bulletForce;

            Destroy(yeniMermi, destroyTime);
            Destroy(yeniEfekt, 0.2f);
            StartCoroutine(FireStandby());
        }
    }

    private IEnumerator FireStandby()
    {
        if (!isDie)
        {
            fireIntervalControl = false;
            yield return new WaitForSeconds(fireInterval);
            fireIntervalControl = true;
        }
    }

    public void Die(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            isDie = true;
            animator.SetBool("isDie", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bulletTurret"))
        {
            health -= 40;
            if (health <= 0)
            {
                isDie = true;
                animator.SetBool("isDie", true);
                Destroy(gameObject, 2f);
            }
        }
    }
}