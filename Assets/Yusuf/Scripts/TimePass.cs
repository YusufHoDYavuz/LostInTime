using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class TimePass : MonoBehaviour
{
    [SerializeField] private float shakeTime;
    [SerializeField] private float shakeScale;
    
    [SerializeField] private GameObject teleportPrefab;

    private CinemachineFreeLook cinemachineFreeLook;
    private Transform firstFollowTransform;

    private void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        firstFollowTransform = cinemachineFreeLook.Follow;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            cinemachineFreeLook.Follow = null;
            ShakeKamera();
        }
    }

    public void ShakeKamera()
    {
        Instantiate(teleportPrefab, firstFollowTransform.position, Quaternion.identity);
        transform.DOShakePosition(shakeTime, shakeScale).OnComplete(() =>
        {
            cinemachineFreeLook.Follow = firstFollowTransform;
        });
    }
}