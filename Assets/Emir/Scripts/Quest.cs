using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public QuestDescription questDescription;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,Vector3.one*0.5f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<QuestManager>().questComplete();
        }
    }
}
