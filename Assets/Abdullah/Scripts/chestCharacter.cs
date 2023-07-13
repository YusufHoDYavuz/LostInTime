using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestCharacter : MonoBehaviour
{
    public bool isHaveKey = false;
    [SerializeField] float distance = 2f;
    [SerializeField] Transform chest;

    [SerializeField] chestController chestController;
    void Start()
    {
        
    }

 
    void Update()
    {
        if (isHaveKey)
        {
            if (Vector3.Distance(gameObject.transform.position,chest.position) < distance)
            {
                  chestController.openChest();
            }
        }
    }
}
