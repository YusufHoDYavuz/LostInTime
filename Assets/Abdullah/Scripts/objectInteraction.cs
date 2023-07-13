using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectInteraction : MonoBehaviour
{
    [SerializeField] float interactionDistance = 2f;


    [SerializeField] chestController chestController;

    [SerializeField] GameObject interactionUI;
    [SerializeField] GameObject player;


    void Interact()
    {
        if (chestController.isOpen)
        {
            interactionUI.SetActive(true);
            gameObject.SetActive(false);
            Debug.Log("aaa");
        }
    }

    private void Start()
    {
        interactionUI.SetActive(false);
    }

    private void Update()
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
}
