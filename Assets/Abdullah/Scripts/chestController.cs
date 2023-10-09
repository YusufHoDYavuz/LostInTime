using UnityEngine;

public class ChestController : MonoBehaviour
{
    public  bool isOpen;

    private Animation animation;
    private BoxCollider boxCollider;
    
    void Awake()
    {
        animation = GetComponent<Animation>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void OpenChest()
    {
        if (!isOpen)
        {
            animation.Play("ChestAnim");
            isOpen = true;
            boxCollider.enabled = false;
        }
    }
}
