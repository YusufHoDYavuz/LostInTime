using UnityEngine;

public class CameraController : MonoBehaviour
{
    float xRotation = 0f;
    public float sensitivity = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up, mouseX);
        transform.Rotate(Vector3.left, mouseY);
    }
}