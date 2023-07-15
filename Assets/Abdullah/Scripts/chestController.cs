using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestController : MonoBehaviour
{
    Animation animation;
    public  bool isOpen = false;
    private BoxCollider boxCollider;
    void Start()
    {
        animation = GetComponent<Animation>();
        boxCollider = GetComponent<BoxCollider>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openChest()
    {
        if (isOpen == false)
        {
            animation.Play("ChestAnim");
            isOpen = true;
            boxCollider.enabled = false;
        }

    }
}
