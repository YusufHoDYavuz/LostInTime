using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10.0f; 
    [SerializeField] private float acceleration = 2.0f; 
    [SerializeField] private float deceleration = 2.0f;
    [SerializeField] private float rotationSpeed = 80.0f;
    private Rigidbody rb;

    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); 
        float verticalInput = Input.GetAxis("Vertical"); 

        if (verticalInput > 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0.0f, deceleration * Time.deltaTime);
        }

        Vector3 movement = transform.forward * currentSpeed;
        rb.velocity = movement;

        float rotation = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);


    }
}