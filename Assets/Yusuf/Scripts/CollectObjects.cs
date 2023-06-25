using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObjects : MonoBehaviour
{
    public float raycastDistance;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Collactable"))
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Collactable Object: " + hit.collider.name);
                }
            }
            
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.cyan);
        }
    }
}
