using UnityEngine;

public class PresentKeyCode : MonoBehaviour
{
    [SerializeField] float interactionDistance = 5f;
    [SerializeField] GameObject player;
    [SerializeField] ChestCharacter chestCharacter;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Interact();
        }
    }

    void Interact()
    {
        chestCharacter.isHaveKey = true;
        Destroy(gameObject, 2f);
    }
}