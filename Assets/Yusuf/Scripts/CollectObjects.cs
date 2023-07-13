using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectObjects : MonoBehaviour
{
    public float raycastDistance;

    [SerializeField] private List<Sprite> parchmentUIList = new List<Sprite>();
    private int parchmentValue;

    [SerializeField] private GameObject parchmentUI;
    private bool isActiveParchmentUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Collactable"))
                {
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Collactable Object: " + hit.collider.name);
                }
                else if (hit.collider.CompareTag("Show"))
                {
                    if (!isActiveParchmentUI)
                    {
                        Sprite matchingSprite = FindSpriteByRaycastObjectName(parchmentUIList, hit.collider.name);

                        parchmentUI.transform.DOLocalMove(Vector3.zero, 0.25f);
                        isActiveParchmentUI = true;

                        if (matchingSprite != null)
                        {
                            parchmentUI.GetComponent<Image>().sprite = parchmentUIList[parchmentValue];
                        }
                    }
                }
                else if (hit.collider.CompareTag("Gem"))
                {
                    if (hit.collider.name == "PastGem")
                    {
                        Singleton.Instance.CallGemAction(5);
                        Singleton.Instance.RaiseGemAmount(1);
                    }
                    else if (hit.collider.name == "PresentGem")
                    {
                        Singleton.Instance.CallGemAction(5);
                        Singleton.Instance.RaiseGemAmount(1);
                    }
                    else if (hit.collider.name == "FutureGem")
                    {
                        Singleton.Instance.CallGemAction(5);
                        Singleton.Instance.RaiseGemAmount(1);
                    }

                    if (Singleton.Instance.gems == 3)
                        Debug.Log("FINISH GAME");

                    Debug.Log("Gem count: " + Singleton.Instance.gems);
                    Destroy(hit.collider.gameObject);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.cyan);
        }

        if (isActiveParchmentUI && Input.GetKeyDown(KeyCode.O))
        {
            parchmentUI.transform.DOLocalMove(new Vector3(0, 1100, 0), 0.25f);
            isActiveParchmentUI = false;
        }
    }

    private Sprite FindSpriteByRaycastObjectName(List<Sprite> sprites, string objectName)
    {
        int raiseValue = 0;

        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == objectName)
            {
                parchmentValue = raiseValue;
                return sprite;
            }

            raiseValue++;
        }

        return null;
    }
}