using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Vector2 = System.Numerics.Vector2;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    private bool isActiveTutorial = true;

    [SerializeField] private GameObject uiPanel;
    private bool isActivePanel;

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

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetActiveTransitionPanel();
        }
    }

    public void SetActiveTransitionPanel()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        uiPanel.transform.DOScale(Vector3.zero, 0.25f).From();
    }
}