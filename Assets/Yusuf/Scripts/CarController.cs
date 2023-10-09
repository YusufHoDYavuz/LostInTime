using UnityEngine;
using System;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    [SerializeField] private List<Wheel> wheels;
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    [SerializeField] private float maxAcceleration = 30.0f;
    [SerializeField] private float brakeAcceleration = 50.0f;
    [SerializeField] private float turnSensitivity = 1.0f;
    [SerializeField] private float maxSteerAngle = 30.0f;
    [SerializeField] private float maxSpeed = 10.0f;
    
    private Vector3 _centerOfMass;

    private float moveInput;
    private float steerInput;

    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void Update()
    {
        GetInputs();
        CarSpeed();
        AnimateWheels();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space) || moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }
    
    private void CarSpeed()
    {
        if (!carRb.isKinematic)
        {
            carRb.velocity = Vector3.ClampMagnitude(carRb.velocity, maxSpeed / 3.6f);
            float carSpeed = carRb.velocity.magnitude * 3.6f;
        }
    }
    
    public enum Axel
    {
        Front,
        Rear
    }
}