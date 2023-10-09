using UnityEngine;

public class TargetForEnemy : MonoBehaviour
{
    public GameObject pointObject;
    [SerializeField] float distance = 3f;
    [SerializeField] Enemy_AI Enemy_AI;

    public void GenerateRandomPoint(GameObject enemy)
    {
        if (Vector3.Distance(transform.position, enemy.transform.position) < Enemy_AI.distanceEnemy)
        {
            Vector3 offset = Random.onUnitSphere * distance;

            Vector3 randomPoint = transform.position + offset;
            randomPoint.y = enemy.transform.position.y;

            pointObject = new GameObject("EmptyObject");
            pointObject.transform.position = randomPoint;
        }
    }
}