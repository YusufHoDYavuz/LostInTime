using UnityEngine;

public class NPCAI : MonoBehaviour
{
    [SerializeField] private AIState currentState = AIState.None;
    [SerializeField] GameObject referanceObject;
    [SerializeField] float distance = 20;

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private Transform target;
    [SerializeField] private float visionAngle = 60f;

    [SerializeField] PlayerController playerController;

    private int currentWaypointIndex = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentState = AIState.Patrolling;
    }

    private void Update()
    {
       
       
        transform.position = new Vector3(transform.position.x,referanceObject.transform.position.y, transform.position.z);
        if (currentState == AIState.Patrolling && currentWaypointIndex < waypoints.Length)
        {
           
            Movement();
            animator.SetBool("isWalk", true);
        }
        if (currentState == AIState.Patrolling && target != null && IsTargetInVision(target))
        {
            currentState = AIState.Detected;
            animator.SetBool("isWalk", false);
            PlayerController.canMove = false;
            StartCoroutine(playerController.gettingCaughtTeleport());
        }
        if (currentWaypointIndex >= waypoints.Length - 1)
        {
            currentWaypointIndex = 0;
            Vector3 newPos = transform.position;
            transform.position = newPos;

        }
    }

    private void Movement()
    {
        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;
        targetDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);


        Vector3 targetPosition = new Vector3(waypoints[currentWaypointIndex].position.x, referanceObject.transform.position.y,
        waypoints[currentWaypointIndex].position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            if (currentWaypointIndex < waypoints.Length)
            {
                currentWaypointIndex++;
                Vector3 newPos = transform.position;

                transform.position = newPos;
               
            }
 
        }
    }

    private bool IsTargetInVision(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        if (angle <= visionAngle && Vector3.Distance(transform.position, target.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public enum AIState
    {
        None,
        Patrolling,
        Detected
    }
}