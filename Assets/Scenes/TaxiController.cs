using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarStats : System.Object
{
    [Range(0, 10)]
    public int topSpeed = 5;
    [Range(0, 10)]
    public int acceleration = 5;
    [Range(0, 10)]
    public int handling = 5;
    [Range(10, 50)]
    public int steerAngle = 30;
    [Range(500, 5000)]
    public int vehicleMass = 2500;
    public Vector3 vehicleCenterOfMass;
}

[System.Serializable]
public class CarAxelGroup : System.Object
{
    public WheelCollider leftWheel;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheel;
    public GameObject rightWheelMesh;
    public bool steering;
    public bool reverseTurn;
}

public class TaxiController : MonoBehaviour
{
    public CarStats stats;
    public bool braking;
    public float currentSteer;
    public float currentSpeed;
    public AnimationCurve accelerationCurve;
    public List<CarAxelGroup> axels;

    private Rigidbody carBody;

    void Awake()
    {
        carBody = GetComponent<Rigidbody>();
        carBody.mass = stats.vehicleMass;
        carBody.centerOfMass = stats.vehicleCenterOfMass;
    }

    void Update()
    {
        foreach (CarAxelGroup carAxel in axels)
        {
            carAxel.leftWheel.motorTorque = currentSpeed * Time.deltaTime * 75000;
            carAxel.rightWheel.motorTorque = currentSpeed * Time.deltaTime * 75000;

            if (carAxel.steering)
            {
                carAxel.leftWheel.steerAngle = currentSteer * stats.steerAngle;
                carAxel.rightWheel.steerAngle = currentSteer * stats.steerAngle;
            }

            if (braking)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime * stats.handling * 3);
                carAxel.leftWheel.brakeTorque += 300 * stats.handling;
                carAxel.rightWheel.brakeTorque += 300 * stats.handling;
            }
            else
            {
                carAxel.leftWheel.brakeTorque = 0;
                carAxel.rightWheel.brakeTorque = 0;
            }

            VisualizeWheel(carAxel);
        }
    }

    public void Steer(float steerFactor)
    {
        currentSteer = Mathf.MoveTowards(currentSteer, steerFactor, Time.deltaTime * stats.handling);
    }

    public void RevertSteering()
    {
        currentSteer = Mathf.MoveTowards(currentSteer, 0, Time.deltaTime * stats.handling * 2);
    }

    public void Accelerate()
    {
        currentSpeed = Mathf.Clamp(currentSpeed, 0, stats.topSpeed);
        currentSpeed += accelerationCurve.Evaluate(Mathf.Abs(currentSpeed) / stats.topSpeed) * Time.deltaTime * stats.acceleration;
    }

    public void Reverse()
    {
        currentSpeed = Mathf.Clamp(currentSpeed, -stats.topSpeed / 2, 0);
        currentSpeed -= accelerationCurve.Evaluate(Mathf.Abs(currentSpeed) / stats.topSpeed) * Time.deltaTime * stats.acceleration;
    }

    public void SlowDown()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime * stats.handling);
    }

    public void VisualizeWheel(CarAxelGroup wheelPair)
    {
        Quaternion rot;
        Vector3 pos;
        wheelPair.leftWheel.GetWorldPose(out pos, out rot);
        wheelPair.leftWheelMesh.transform.position = pos;
        wheelPair.leftWheelMesh.transform.rotation = rot;
        wheelPair.rightWheel.GetWorldPose(out pos, out rot);
        wheelPair.rightWheelMesh.transform.position = pos;
        wheelPair.rightWheelMesh.transform.rotation = rot;
    }
}