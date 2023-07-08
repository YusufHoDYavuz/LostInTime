using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    private bool isActiveTutorial = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {

            if (isActiveTutorial)
            {
                tutorial.transform.DOLocalMove(new Vector3(1250, 0, 0), 0.25f);
            }
            else
            {
                tutorial.transform.DOLocalMove(new Vector3(680, 0, 0), 0.25f);
            }
            
            isActiveTutorial = !isActiveTutorial;
           
        }
    }
}
