using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchColliders : MonoBehaviour
{
    private BoxCollider col;
    public void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider hit)
    {
        Debug.Log("Enemy hit");
        if (hit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit");
            hit.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            hit.transform.gameObject.GetComponent<Enemy_AI>().Die(100);
            Destroy(hit.transform.gameObject, 10f);
        }
            col.enabled = false;
    }

}
