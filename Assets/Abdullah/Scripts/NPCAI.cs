using UnityEngine;

public class NPCAI : MonoBehaviour
{
    [SerializeField] private AIState currentState = AIState.None;
    
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private Transform target;
    [SerializeField] private float visionAngle = 60f;

    private int currentWaypointIndex = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentState = AIState.Patrolling;
    }

    private void Update()
    {
        if (currentState == AIState.Patrolling && currentWaypointIndex < waypoints.Length)
        {
            Movement();
            animator.SetBool("isWalk", true);
        }

        if (currentState == AIState.Patrolling && target != null && IsTargetInVision(target))
        {
            currentState = AIState.Detected;
            animator.SetBool("isWalk", false);
        }
    }

    private void Movement()
    {
        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;
        targetDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);


        Vector3 targetPosition = new Vector3(waypoints[currentWaypointIndex].position.x, transform.position.y,
            waypoints[currentWaypointIndex].position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            currentWaypointIndex++;
            Vector3 newPos = transform.position;
            newPos.y = 0f;
            transform.position = newPos;
        }
    }

    private bool IsTargetInVision(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        if (angle <= visionAngle)
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