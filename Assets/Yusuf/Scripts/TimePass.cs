using System.Collections;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class TimePass : MonoBehaviour
{
    [SerializeField] private float shakeTime;
    [SerializeField] private float shakeScale;

    [SerializeField] private float transitionWaitTime;

    [SerializeField] private GameObject teleportPrefab;

    private CinemachineFreeLook cinemachineFreeLook;
    private Transform firstFollowTransform;

    private void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        firstFollowTransform = cinemachineFreeLook.Follow;
    }

    public void ShakeKamera(int sceneIndex)
    {
        cinemachineFreeLook.Follow = null;
        Instantiate(teleportPrefab, firstFollowTransform.position, Quaternion.identity);
        transform.DOShakePosition(shakeTime, shakeScale).OnComplete(() =>
        {
            cinemachineFreeLook.Follow = firstFollowTransform;
        });

        StartCoroutine(TransitionTime(transitionWaitTime, sceneIndex));
    }

    private IEnumerator TransitionTime(float waitTime, int sceneIndex)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneIndex);
    }
}