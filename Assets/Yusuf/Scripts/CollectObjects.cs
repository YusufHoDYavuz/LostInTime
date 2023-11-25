using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CollectObjects : MonoBehaviour
{
    public float raycastDistance;

    [SerializeField] private List<Sprite> parchmentUIList = new List<Sprite>();
    private int parchmentValue;

    [SerializeField] private GameObject parchmentUI;
    private bool isActiveParchmentUI;

    [Header("Keypad")] private string currentPassword = "";
    private string correctPassword = "1328";
    [SerializeField] private Transform[] doors;

    private DragAndDropController dragAndDropController;

    private bool isActiveCar;

    [SerializeField] private GameObject mainPlayerObject;
    [SerializeField] private SkinnedMeshRenderer capeMeshRenderer;

    [SerializeField] private GameObject collactableUICount;
    [SerializeField] private GameObject autoCollactable;
    [SerializeField] private Text collactableText;
    [SerializeField] private Transform pastChestLid;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    public void openDoor(int doorIndex)
    {
        doors[doorIndex].DOLocalMoveY(doors[doorIndex].transform.localPosition.y + 8 - doorIndex * 7, 1f);
    }

    private void Start()
    {
        dragAndDropController = GetComponent<DragAndDropController>();
        if (Singleton.Instance.purchasedItems[2])
            capeMeshRenderer.enabled = true;
        if (Singleton.Instance.purchasedItems[0])
            DragAndDropController.isTurretPurchased = true;

        collactableText.text = "Robot Parçaları: " + Singleton.Instance.collactableCount;
    }

    void Update()
    {
        if (Singleton.Instance.rotationFinished)
        {
            Debug.Log("Chest Open");
            Singleton.Instance.rotationFinished = false;
            playerController.gameObject.GetComponent<QuestManager>().questComplete();
            PastRotationFinish();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Show"))
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

                        switch (hit.collider.name)
                        {
                            case "p1":
                                Singleton.Instance.pastPuzzlesScrollCount[0] = true;
                                PastPuzzleFinish();
                                break;
                            case "p2":
                                Singleton.Instance.pastPuzzlesScrollCount[1] = true;
                                PastPuzzleFinish();
                                break;
                            case "p3":
                                Singleton.Instance.pastPuzzlesScrollCount[2] = true;
                                PastPuzzleFinish();
                                break;
                            case "p4":
                                Singleton.Instance.pastPuzzlesScrollCount[3] = true;
                                PastPuzzleFinish();
                                break;
                        }
                    }
                }
                else if (hit.collider.CompareTag("Gem"))
                {
                    if (hit.collider.name == "PastGem")
                    {
                        Singleton.Instance.CallGemAction(5, 0);
                        Singleton.Instance.RaiseGemAmount(1);
                        playerController.gameObject.GetComponent<QuestManager>().questComplete();
                    }
                    else if (hit.collider.name == "PresentGem")
                    {
                        Singleton.Instance.CallGemAction(5, 1);
                        Singleton.Instance.RaiseGemAmount(1);
                    }
                    else if (hit.collider.name == "FutureGem")
                    {
                        Singleton.Instance.CallGemAction(5, 2);
                        Singleton.Instance.RaiseGemAmount(1);
                        openDoor(1);
                    }

                    if (Singleton.Instance.gems == 3)
                        Singleton.Instance.gameFinished = true;

                    Debug.Log("Gem count: " + Singleton.Instance.gems);
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Purchaseable"))
                {
                    string itemName = hit.collider.name;
                    switch (itemName)
                    {
                        case "Turret":

                            if (!Singleton.Instance.purchasedItems[0] && Singleton.Instance.collactableCount >= 75)
                            {
                                Singleton.Instance.collactableCount -= 75;
                                Singleton.Instance.purchasedItems[0] = true;
                                DragAndDropController.isTurretPurchased = true;
                            }

                            break;

                        case "microchip":
                            if (!Singleton.Instance.purchasedItems[1] && Singleton.Instance.collactableCount >= 25)
                            {
                                Singleton.Instance.collactableCount -= 25;
                                Singleton.Instance.purchasedItems[1] = true;
                                Singleton.Instance.speedMultiplier = 1.5f;
                            }

                            break;
                        case "Cape":
                            if (!Singleton.Instance.purchasedItems[2] && Singleton.Instance.collactableCount >= 25)
                            {
                                Singleton.Instance.collactableCount -= 25;
                                Singleton.Instance.purchasedItems[2] = true;
                                capeMeshRenderer.enabled = true;
                            }

                            break;

                        default:
                            Debug.Log("No item purchased");
                            break;
                    }
                }
                else if (hit.collider.CompareTag("keypad"))
                {
                    string keypadValue = hit.collider.name;
                    if (keypadValue == "ok")
                    {
                        if (currentPassword == correctPassword)
                        {
                            Debug.Log("Password is correct");
                            openDoor(0);
                        }
                        else
                        {
                            Debug.Log("Password is wrong");
                        }
                    }
                    else if (keypadValue == "clear")
                    {
                        currentPassword = "";
                    }
                    else
                    {
                        currentPassword += keypadValue;
                    }
                }
                else if (hit.collider.CompareTag("Car"))
                {
                    dragAndDropController.cmFreeLook.Follow = dragAndDropController.carPov;
                    dragAndDropController.cmFreeLook.LookAt = dragAndDropController.carPov;
                    isActiveCar = true;
                    dragAndDropController.carController.enabled = true;
                    dragAndDropController.player.SetActive(false);
                    dragAndDropController.player.transform.parent = dragAndDropController.carPov.transform;
                }
                else if (hit.collider.CompareTag("oldMan"))
                {
                    mainPlayerObject.GetComponentInChildren<DialogueManager>().PlayOldMan();
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.cyan);
        }

        if (isActiveParchmentUI && Input.GetKeyDown(KeyCode.Q))
        {
            parchmentUI.transform.DOLocalMove(new Vector3(0, 1100, 0), 0.25f);
            isActiveParchmentUI = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && isActiveCar)
        {
            dragAndDropController.carController.enabled = false;
            dragAndDropController.cmFreeLook.Follow = dragAndDropController.currentPov;
            dragAndDropController.cmFreeLook.LookAt = dragAndDropController.currentPov;
            dragAndDropController.player.SetActive(true);
            isActiveCar = false;
            dragAndDropController.player.transform.parent = mainPlayerObject.transform;
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

    private IEnumerator CollactableUIActive(float waitTime)
    {
        collactableUICount.transform.DOLocalMoveX(-750, 0.5f).OnComplete(() =>
        {
            collactableText.gameObject.transform.DOScale(Vector3.one * 2, 0.5f).OnComplete(() =>
            {
                Singleton.Instance.RaiseCollactableCount(1);
                collactableText.text = "Robot Parçaları: " + Singleton.Instance.collactableCount;
                collactableText.gameObject.transform.DOScale(Vector3.one * 1.5F, 0.5f);
            });
        });
        yield return new WaitForSeconds(waitTime);
        collactableUICount.transform.DOLocalMoveX(-1150, 0.5f);
    }

    void PastPuzzleFinish()
    {
        if (Singleton.Instance.pastPuzzlesScrollCount[0] && Singleton.Instance.pastPuzzlesScrollCount[1] &&
            Singleton.Instance.pastPuzzlesScrollCount[2] && Singleton.Instance.pastPuzzlesScrollCount[3])
        {
            openDoor(0);
            mainPlayerObject.GetComponentInChildren<QuestManager>().questComplete();

        }
    }

    void PastRotationFinish()
    {
        pastChestLid.DOLocalRotate(Vector3.right * -90, 1f);
    }

    private void PlayPickUpAnimation()
    {
        playerController.enabled = false;
        Invoke(nameof(SetActivePlayerController), 1.2f);
        animator.SetTrigger("PickUp");
    }

    private void SetActivePlayerController()
    {
        playerController.enabled = true;
    }

    public void SetAutoCollactable(bool isShow)
    {
        if (isShow)
            autoCollactable.transform.DOLocalMoveX(-750, 0.25f);
        else
            autoCollactable.transform.DOLocalMoveX(-1150, 0.25f);
    }

    public void AutoCollactable()
    {
        StartCoroutine(CollactableUIActive(3f));
        PlayPickUpAnimation();
        SetAutoCollactable(false);
    }
}