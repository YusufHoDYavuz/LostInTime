using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class New_enemy_test : MonoBehaviour
{
    GameObject player;
    Animator animator;

    float distance, angle;

    bool isDedection = false;

    [Header("Tespit Mesafeleri")]
    [SerializeField] float frontShortRange;
    [SerializeField] int backShortRange;
    [SerializeField] int sideShortRange;

    [SerializeField] float frontLongRange, backLongRange, sideLongRange;

    [Header("Tespit Açýlarý")]
    [SerializeField] int frontAngle;
    [SerializeField] int backAngle;

    [Header("Tespit Süreleri")]
    [SerializeField] float timeFrontLong;
    [SerializeField] float timeFrontShort;
    [SerializeField] float timeBackLong;
    [SerializeField] float timeBackShort;
    [SerializeField] float timeSideLong;
    [SerializeField] float timeSideShort;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float destroyTime = 10f;
    [SerializeField] private float fireInterval = 0.5f;
    [SerializeField] GameObject fireEffect;

    [SerializeField] private bool isFire;
    bool fireIntervalControl = true;


    [SerializeField] Transform[] patrolPoints;
    Vector3 nextPatrolPoint ;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float maxFireDistance = 15;
    [SerializeField] float maxDedectionDistance = 35;


    [SerializeField] int health = 100;
    bool isDie = false;

    private void Start()
    {
         player = GameObject.FindWithTag("Player");
        nextPatrolPoint = transform.position;
        GenerateRandomPoint();
    }

    void Update()
    {
        calculateValues();
        checkAngle();
        Dedection();
        Rotate();
        if (isFire && fireIntervalControl)
            Fire();
    }

    void calculateValues()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        Vector3 direction = transform.position - player.transform.position;
        angle = Vector3.Angle(direction, gameObject.transform.forward);      
    }

    void checkAngle()
    {
        if (angle < 0 + (frontAngle/2) || angle > 360 - (frontAngle/2))
        {
            if (distance < frontShortRange)
            {
                Debug.Log("önünde");
                StartCounter(timeFrontShort);
            }
            else if (distance < frontLongRange)
            {
                Debug.Log("önünde uzun");
                StartCounter(timeFrontLong);
            }
            else
            {
                StopAllCoroutines();
            }
            
        }else if (angle < 180 - backAngle/2 && angle > frontAngle/2 || angle > 180 + backAngle/2 && angle < 360 - frontAngle/2)
        {
            if (distance < sideShortRange)
            {
                Debug.Log("yanda");
                StartCounter(timeSideShort);
            }
            else if (distance < sideLongRange)
            {
                Debug.Log("Yanda uzun");
                StartCounter(timeSideLong);
            }
            else
            {
                StopAllCoroutines();
            }
        }
        else
        {
            if (distance < backShortRange)
            {
                Debug.Log("arkada");
                StartCounter(timeBackShort);
            }
            else if (distance < backLongRange)
            {
                Debug.Log("arkada uzun");
                StartCounter(timeBackLong);
            }
            else
            {
                    StopAllCoroutines();
            }
        }

    }

    void Dedection()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > maxDedectionDistance)
        {
            isDedection = false;
        }
    }

    void StartCounter(float time)
    {
       
        StartCoroutine(Counter(time));
    }

    IEnumerator Counter(float waitingSeconds)
    {
        
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(waitingSeconds);
        isDedection = true;

    }

    private void Rotate()
    {
        if (!isDie)
        {
            if (isDedection)
            {
                //Playera dön
                isFire = true;
                Vector3 targetDirection = player.transform.position - transform.position;
                targetDirection.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
                Movement(targetDirection);

            }
            else if (Vector3.Distance(transform.position, nextPatrolPoint) > 0.5f )
            {
                // Noktaya dön
                isFire = false;
                Vector3 targetDirection = nextPatrolPoint - transform.position;
                
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
                Movement(targetDirection);  
                
            }
            else
            {
                  GenerateRandomPoint();
            }


        }
    }

    private void Movement(Vector3 nextPosition)
    {
        if (!isDedection) 
        {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(transform.position, nextPosition) > maxFireDistance)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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
    void GenerateRandomPoint()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(patrolPoints[0].position.x, patrolPoints[1].position.x),
            transform.position.y,
            Random.Range(patrolPoints[0].position.z, patrolPoints[3].position.z)
        );

            nextPatrolPoint = randomPoint;
        Debug.Log(nextPatrolPoint);
    }
}
