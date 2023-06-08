using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructObject : MonoBehaviour
{
     public GameObject fragmentedObject;
    public GameObject efekt;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            destruct();
            
        }
    }

    void destruct() 
    {
        Instantiate(fragmentedObject,transform.position , transform.rotation);
        Instantiate(efekt, transform.position, transform.rotation);
        Destroy(gameObject);
    } 
}
