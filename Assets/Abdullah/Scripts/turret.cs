using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float rotationSpeed = 5f;

    [SerializeField] GameObject fireEffect;
    [SerializeField] float visionAngle;


    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRange;

    [SerializeField] float fireInterval;

    [SerializeField] float fireForce = 1000;
    bool isTargetInArea = false;

    [SerializeField] GameObject player;


    public bool isFire = false ;
    


    void Start()
    {
        InvokeRepeating("fire", 0f, fireInterval);
       
    }
    void Update()
    {
        returnTarget();
        if (Input.GetKeyDown(KeyCode.X))
        {
            isFire = !isFire;
        }
    }
    private void returnTarget()
    {
        if (IsTargetInVision(target))
        {
            Vector3 direction = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = player.transform.rotation;
        }
    }
    public void fire()
    {
        if (isFire)
        {
            if (IsTargetInVision(target))
            {
                Vector3 direction = target.position - transform.position;


                if (direction.magnitude < fireRange)
                {
                    Vector3 lookingDirection = transform.forward;
                    GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    newBullet.GetComponent<Rigidbody>().AddForce(lookingDirection * fireForce);
                    Instantiate(fireEffect, firePoint.position, Quaternion.identity);
                }
                else
                {
                    isTargetInArea = false;
                }
            }
        }
       
    }


    private bool IsTargetInVision(Transform target)
    {
        Vector3 directionToTarget = target.position - player.transform.position;
        float angle = Vector3.Angle(player.transform.right, directionToTarget);

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