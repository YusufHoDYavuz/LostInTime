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

    [Header("Tespit A��lar�")]
    [SerializeField] int frontAngle;
    [SerializeField] int backAngle;

    [Header("Tespit S�releri")]
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

    private bool isRayActive = false;
    private float timer = 0f;
   


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
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
        {
            // I��n� a� veya kapa
            isRayActive = true;
        }
        else
        {
            isRayActive= false;
        }

        // E�er ���n a��ksa
        if (isRayActive)
        {
            timer += Time.deltaTime;

            
            if (timer >= fireInterval)
            {
                Fire();
                timer = 0f;
            }
        }

          
        
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
                Debug.Log("�n�nde");
                StartCounter(timeFrontShort);
            }
            else if (distance < frontLongRange)
            {
                Debug.Log("�n�nde uzun");
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
                //Playera d�n
                isFire = true;
                Vector3 targetDirection = player.transform.position - transform.position;
                targetDirection.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
                Movement(targetDirection);

                animator.SetBool("isWalk", false);


            }
            else if (Vector3.Distance(transform.position, nextPatrolPoint) > 0.5f )
            {
                // Noktaya d�n
                isFire = false;
                Vector3 targetDirection = nextPatrolPoint - transform.position;
                
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
                Movement(targetDirection);

                animator.SetBool("isWalk", true);

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
            animator.SetBool("isWalk", true);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
                transform.Translate(Vector3.forward * 0 * Time.deltaTime);
            animator.SetBool("isWalk", false);
        }
    }

    void Fire()
    {
        if (!isDie)
        {
            Debug.Log("firee");
          
            // Nesnenin bakt��� y�ne do�ru bir vekt�r olu�tur
            Vector3 atisYonu = transform.forward;

            // Raycast ���n�n� olu�tur
            Ray ray = new Ray(firePoint.position, atisYonu);
            RaycastHit hit;

            // I��n�n �arpt��� nesneyi kontrol et
            if (Physics.Raycast(ray, out hit))
            {
                // �arpan nesnenin ad�n� konsola yazd�r
                Debug.Log("�arp�lan Nesne: " + hit.collider.gameObject.name);
            }
            Debug.DrawRay(firePoint.position, atisYonu * 100f, Color.red, 1f);


            
           
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
