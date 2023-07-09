using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform spine;
    [SerializeField] private float maxDistance;
 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         float distance = Vector3.Distance(transform.position, target.position);
        
         if(distance <= maxDistance)
         {
            spine.LookAt(target.position);
            spine.Rotate(0, 45, 0);
              //transform.Translate(Vector3.forward * Time.deltaTime);
         }
    }
}
