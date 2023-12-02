using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    void Start()
    {
        if (Singleton.Instance.isFirstSceneDone)
        {
            gameObject.SetActive(false);
            return;
        }


            DontDestroyOnLoad(gameObject);
    }
}
