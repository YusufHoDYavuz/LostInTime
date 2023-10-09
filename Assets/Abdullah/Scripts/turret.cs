using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] string enemyTag = "Enemy";
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] GameObject fireEffect;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject firePoint;
    [SerializeField] float fireRange;

    [SerializeField] float fireInterval;

    [SerializeField] float fireForce = 1000;

    private Vector3 direction;
    private GameObject target;

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); 

        float closestDistance = Mathf.Infinity; 
        GameObject closestEnemy = null; 

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy =
                Vector3.Distance(transform.position,
                    enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy; 
                closestEnemy = enemy; 
            }
        }

        target = closestEnemy; 
    }
}