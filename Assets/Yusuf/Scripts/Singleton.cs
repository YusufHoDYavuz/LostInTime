using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;

    public int gems;
    public GameObject gemsParentUI;
    public List<GameObject> gemUIList = new List<GameObject>();

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

    public void CallGemAction(float waitTime)
    {
        StartCoroutine(GetGemUIAndSetGems(waitTime));
    }

    private IEnumerator GetGemUIAndSetGems(float waitTime)
    {
        gemsParentUI.transform.DOLocalMove(new Vector3(0, 450, 0), 0.25f).OnComplete(() =>
        {
            if (gems == 1)
            {
                gemUIList[0].transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.InBounce);
            }
            else if (gems == 2)
            {
                gemUIList[1].transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.InBounce);;
            }
            else if (gems == 3)
            {
                gemUIList[2].transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.InBounce);;
            }
        });
        yield return new WaitForSeconds(waitTime);
        gemsParentUI.transform.DOLocalMove(new Vector3(0, 600, 0), 0.25f);
    }
}