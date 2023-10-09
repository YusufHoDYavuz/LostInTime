using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] float interactionDistance = 2f;
    [SerializeField] ChestController chestController;
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