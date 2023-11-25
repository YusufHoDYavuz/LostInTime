using UnityEngine;

public class RotationPuzzle : MonoBehaviour
{
    private bool isInteracting;
    private GameObject[] puzzlePieces;
    private Camera cam;
    public static int puzzlePiecesLength;

    void Start()
    {
        puzzlePieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");
        puzzlePiecesLength = puzzlePieces.Length;
        cam = Camera.main;
    }

    void Update()
    {
        InteractPiece();

        if (!isInteracting)
        {
            foreach (GameObject piece in puzzlePieces)
            {
                Outline objectOutline = piece.GetComponent<Outline>();
                if (objectOutline != null)
                {
                    objectOutline.enabled = false;
                }
            }
        }
    }

    void InteractPiece()
    {
        RaycastHit hit;

        Vector3 raycastOrigin = cam.transform.position + cam.transform.forward;
        Ray ray = new Ray(raycastOrigin, cam.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "PuzzlePiece")
            {
                Outline objectOutline = hit.collider.gameObject.GetComponent<Outline>();
                if (objectOutline != null)
                {
                    objectOutline.enabled = true;
                    isInteracting = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.gameObject.GetComponent<PuzzlePiece>().RotatePiece();
                        if (Singleton.Instance.rotationFinished)
                            GetComponent<QuestManager>().questComplete();
                    }
                }
            }
            else
                isInteracting = false;
        }
    }
}