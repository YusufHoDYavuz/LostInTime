using Unity.VisualScripting;
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

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 raycastOrigin = transform.position + transform.forward;
            Ray ray = new Ray(raycastOrigin, transform.forward);

            if (Physics.Raycast(ray, out hit , rayDistance))
            {
                if (hit.collider.gameObject.CompareTag("DraggableObject"))
                {
                    isDragging = true;
                    draggedObject = hit.collider.gameObject;
                    initialDistance = Vector3.Distance(transform.position, draggedObject.transform.position);
                    SetActiveDragObject(false);
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                draggedObject = null;
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
        hit.collider.GetComponent<Rigidbody>().useGravity = isActive;
        hit.collider.GetComponent<Collider>().isTrigger = !isActive;
    }
}