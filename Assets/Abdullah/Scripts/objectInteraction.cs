using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectInteraction : MonoBehaviour
{
    [SerializeField] float interactionDistance = 3f;


    [SerializeField] string dialogueText = "Merhaba! Nasýlsýnýz?";

    [SerializeField] GameObject interactionUI;
    [SerializeField] GameObject player;


    void Interact()
    {
        Debug.Log(dialogueText);
        // fonskiyonunu buraya yazcaz
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
            interactionUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
                interactionUI.SetActive(false);
            }
        }
        else
        {
            interactionUI.SetActive(false);
        }
    }
}
