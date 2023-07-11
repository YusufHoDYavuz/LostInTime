using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;

    public int gems;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Singleton Instance
    {
        get { return instance; }
    }

    public void RaiseGemAmount(int raiseValue)
    {
        gems += raiseValue;
    }
}