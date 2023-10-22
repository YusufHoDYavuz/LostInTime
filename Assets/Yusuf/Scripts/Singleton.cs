using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;
    public int gems;
    public GameObject gemsParentUI;
    public List<GameObject> gemUIList = new();
    public bool[] purchasedItems = { false, false, false };
    public float speedMultiplier = 1f;

    public int collactableCount;
    public bool gameFinished;
    public bool[] pastPuzzlesScrollCount = { false, false, false, false };
    public bool rotationFinished;
    public Collider[] punchColliders;

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

    public void RaiseCollactableCount(int raiseValue)
    {
        collactableCount += raiseValue;
    }

    public void CallGemAction(float waitTime, int gemIndex)
    {
        StartCoroutine(GetGemUIAndSetGems(waitTime, gemIndex));
    }

    private IEnumerator GetGemUIAndSetGems(float waitTime, int gemIndex)
    {
        gemsParentUI.transform.DOLocalMove(new Vector3(0, 450, 0), 0.25f).OnComplete(() =>
        {
            gemUIList[gemIndex].transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.InBounce);
        });
        yield return new WaitForSeconds(waitTime);
        gemsParentUI.transform.DOLocalMove(new Vector3(0, 600, 0), 0.25f);
    }
}