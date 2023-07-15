using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class presentKeyCode : MonoBehaviour
{

    [SerializeField] float interactionDistance = 5f;
    [SerializeField] GameObject player;
    [SerializeField] chestCharacter chestCharacter;
   

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= interactionDistance)
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
                
            }
        }
      
    }

    void Interact()
    {
       chestCharacter.isHaveKey = true;
       Destroy(gameObject,2f);
    }
}
