using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchColliders : MonoBehaviour
{
    private BoxCollider col;
    [SerializeField] LayerMask allowed;
    public void Start()
    {
        col = GetComponent<BoxCollider>();
    }


    public void OnTriggerEnter (Collider hit)
    {
        Debug.Log(hit.name);
        if (allowed == (allowed | (1 << hit.gameObject.layer)))
        {   
            Debug.Log("Enemy hit");
            hit.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            hit.transform.gameObject.GetComponent<New_enemy_test>().Die(100);
            Destroy(hit.transform.gameObject, 10f);
        }
            col.enabled = false;
    }

}
