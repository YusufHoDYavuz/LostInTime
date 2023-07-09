using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    GameObject target;
    [SerializeField] string enemyTag = "Enemy";

    [SerializeField] float rotationSpeed = 5f;

    [SerializeField] GameObject fireEffect;



    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject firePoint;
    [SerializeField] float fireRange;

    [SerializeField] float fireInterval;

    [SerializeField] float fireForce = 1000;
    Vector3 direction;





    void Start()
    {
        InvokeRepeating("fire", 0f, fireInterval);

    }
    void Update()
    {
        FindClosestEnemy();
        returnTarget();
      
    }
    private void returnTarget()
    {
        direction = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    public void fire()
    {
        

        if (direction.magnitude < fireRange)
        {
            Vector3 lookingDirection = transform.forward;
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(lookingDirection * fireForce);
            Instantiate(fireEffect, firePoint.transform.position, Quaternion.identity);
        }


    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); // Enemy etiketine sahip tüm nesneleri al

        float closestDistance = Mathf.Infinity; // En yakýn mesafeyi sonsuz olarak baþlat
        GameObject closestEnemy = null; // En yakýn düþmaný null olarak baþlat

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); // Hedef ile düþman arasýndaki mesafeyi hesapla

            if (distanceToEnemy < closestDistance) // Eðer bu düþman, en yakýn düþmandan daha yakýnsa
            {
                closestDistance = distanceToEnemy; // En yakýn mesafeyi güncelle
                closestEnemy = enemy; // En yakýn düþmaný güncelle
            }
        }

        target = closestEnemy; // Hedefi en yakýn düþman olarak ayarla
    }
}





        
