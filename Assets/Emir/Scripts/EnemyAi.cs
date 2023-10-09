using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform spine;
    [SerializeField] private float maxDistance;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= maxDistance)
        {
            spine.LookAt(target.position);
            spine.Rotate(0, 45, 0);
        }
    }
}