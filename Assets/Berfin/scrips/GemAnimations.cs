using UnityEngine;
using DG.Tweening;

public class GemAnimations : MonoBehaviour
{
    [SerializeField] private GameObject gem1;
    [SerializeField] private GameObject gem2;
    [SerializeField] private GameObject gem3;
    
    void Start()
    {
        GemTween(gem1);
        GemTween(gem2);
        GemTween(gem3);
    }

    private void GemTween(GameObject gem)
    {
        gem.transform.DOLocalRotate(new Vector3(0, 360, 0), 5f).SetLoops(-1, LoopType.Yoyo);
    }
}
