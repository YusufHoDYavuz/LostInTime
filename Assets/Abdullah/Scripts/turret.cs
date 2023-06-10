using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    [SerializeField] Transform target; // The target object to follow
    [SerializeField] float rotationSpeed = 5f; // Rotation speed variable

    [SerializeField] GameObject fireEffect;


    [SerializeField] GameObject bulletPrefab; // Kullanýlacak mermi örneði
    [SerializeField] Transform firePoint; // Mermi çýkýþ noktasý
    [SerializeField] float fireRange;

    [SerializeField] float fireInterval;

    [SerializeField] float fireForce = 1000;
    bool isTargetInArea = false;


    private void Start()
    {
        InvokeRepeating("fire", 0f, fireInterval);
    }
    void Update()
    {
        foundTarget();
    }

    private void foundTarget()
    {

        if (target != null)
        {

            Vector3 direction = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (direction.magnitude < fireRange)
            {
                isTargetInArea = true;
            }
            else
            {
                isTargetInArea = false;
            }
        }
    }




    void fire()
    {
        if (isTargetInArea)
        {

            Vector3 lookingDirection = transform.forward;
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(lookingDirection * fireForce);
            Instantiate(fireEffect, firePoint.position, Quaternion.identity);
        }
    }

}
