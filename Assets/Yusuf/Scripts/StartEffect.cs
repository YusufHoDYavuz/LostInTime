using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5f);
    }
}
