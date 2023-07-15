using Cinemachine;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    //DRAG-DROP
    private bool isDragging = false;
    private GameObject draggedObject;

    //RAYCAST
    [SerializeField] private float rayDistance;
    private float initialDistance;
    private RaycastHit hit;
    
    //DRAG DROP POV
    public CinemachineFreeLook cmFreeLook;
    public Transform currentPov;
    public Transform carPov;
    public GameObject player;
    public CarController carController;
    [SerializeField] private Transform dragDropPov;

    //************************

    [SerializeField] private GameObject turretPrefab; 
    [SerializeField] private float minDistance = 0.5f; 
    [SerializeField] private float maxDistance = 10f; 

    private bool isRaycasting = false; 
    private RaycastHit raycastHit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
           
            isRaycasting = true;
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
           
            isRaycasting = false;

            if (!isRaycasting)
            {
                Vector3 raycastOrigin = transform.position + transform.forward;
                Ray ray = new Ray(raycastOrigin, transform.forward);

                Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

                if (Physics.Raycast(ray, out raycastHit, maxDistance))
                {

                    float distance = raycastHit.distance;
                    if (distance >= minDistance && distance <= maxDistance)
                    {
                        turretPrefab.transform.position = raycastHit.point;
                        Instantiate(turretPrefab);
                    }
                }
            }
        }


        //************************************************************************
        if (Input.GetMouseButton(1))
        {
            Vector3 raycastOrigin = transform.position + transform.forward;
            Ray ray = new Ray(raycastOrigin, transform.forward);

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.gameObject.CompareTag("DraggableObject"))
                {
                    isDragging = true;
                    draggedObject = hit.collider.gameObject;
                    initialDistance = Vector3.Distance(transform.position, draggedObject.transform.position);
                    cmFreeLook.Follow = dragDropPov;
                    cmFreeLook.LookAt = dragDropPov;
                    SetActiveDragObject(false);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (isDragging)
            {
                isDragging = false;
                draggedObject = null;
                cmFreeLook.Follow = currentPov;
                cmFreeLook.LookAt = currentPov;
                SetActiveDragObject(true);
            }
        }

        if (isDragging && draggedObject != null)
        {
            Vector3 targetPosition = transform.position + (transform.forward * initialDistance);
            draggedObject.transform.position = targetPosition;
        }
    }

    private void SetActiveDragObject(bool isActive)
    {
        if (hit.collider.GetComponent<Rigidbody>() != null)
            hit.collider.GetComponent<Rigidbody>().useGravity = isActive;

        if (hit.collider.GetComponent<Collider>() != null)
            hit.collider.GetComponent<Collider>().isTrigger = !isActive;
    }
}