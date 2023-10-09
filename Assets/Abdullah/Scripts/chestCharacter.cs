using UnityEngine;

public class ChestCharacter : MonoBehaviour
{
    public bool isHaveKey = false;
    [SerializeField] float distance = 2f;
    [SerializeField] Transform chest;

    [SerializeField] ChestController chestController;

    void Update()
    {
        if (isHaveKey)
        {
            if (Vector3.Distance(gameObject.transform.position, chest.position) < distance)
                chestController.OpenChest();
        }
    }
}