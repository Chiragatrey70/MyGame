using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Meshes")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("Car Settings")]
    [Range(500f, 5000f)] public float motorTorque = 1500f;
    [Range(10f, 45f)] public float maxSteerAngle = 30f;
    [Range(500f, 8000f)] public float brakeForce = 3000f;
    [Range(0f, 2000f)] public float decelerationForce = 500f;

    [Header("Drive Type")]
    public bool frontWheelDrive = false;
    public bool rearWheelDrive = true;
    public bool allWheelDrive = false;

    [Header("Stability Settings")]
    public Vector3 centerOfMassOffset = new Vector3(0, -0.3f, 0);
    [Range(1000f, 10000f)] public float antiRollForce = 5000f;
    [Range(0.5f, 5f)] public float wheelStiffness = 1.7f;

    [Header("UI")]
    public Text speedText;

    // --- Variables for touch controls ---
    private float steerInput;
    private float gasInput;
    private float brakeInput;

    // --- Private variables ---
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;
        SetWheelFriction();
    }

    void Update()
    {
        UpdateWheelVisuals();
        UpdateSpeedUI();
    }

    void FixedUpdate()
    {
        ApplyMotorTorque();
        ApplySteering();
        ApplyBrakes();
        ApplyAntiRoll(frontLeftWheel, frontRightWheel);
        ApplyAntiRoll(rearLeftWheel, rearRightWheel);
    }

    // --- Public methods for button events ---
    public void StartSteeringLeft() { steerInput = -1; }
    public void StartSteeringRight() { steerInput = 1; }
    public void StopSteering() { steerInput = 0; }

    public void StartAccelerating() { gasInput = 1; }
    public void StopAccelerating() { gasInput = 0; }

    public void StartReversing() { gasInput = -1; }
    public void StopReversing() { gasInput = 0; }

    public void StartBraking() { brakeInput = 1; }
    public void StopBraking() { brakeInput = 0; }


    void ApplyMotorTorque()
    {
        float torque = gasInput * motorTorque;

        if (frontWheelDrive || allWheelDrive)
        {
            frontLeftWheel.motorTorque = torque;
            frontRightWheel.motorTorque = torque;
        }

        if (rearWheelDrive || allWheelDrive)
        {
            rearLeftWheel.motorTorque = torque;
            rearRightWheel.motorTorque = torque;
        }

        if (gasInput == 0 && brakeInput == 0)
        {
            frontLeftWheel.brakeTorque = decelerationForce;
            frontRightWheel.brakeTorque = decelerationForce;
            rearLeftWheel.brakeTorque = decelerationForce;
            rearRightWheel.brakeTorque = decelerationForce;
        }
        else
        {
            frontLeftWheel.brakeTorque = 0;
            frontRightWheel.brakeTorque = 0;
            rearLeftWheel.brakeTorque = 0;
            rearRightWheel.brakeTorque = 0;
        }
    }

    void ApplySteering()
    {
        float speed = rb.linearVelocity.magnitude;
        float steerFactor = Mathf.Clamp01(speed / 50f);
        float adjustedSteer = Mathf.Lerp(maxSteerAngle, maxSteerAngle * 0.6f, steerFactor);
        float steerAngle = steerInput * adjustedSteer; // Corrected variable name

        // Apply steering to both front wheels
        frontLeftWheel.steerAngle = steerAngle;
        frontRightWheel.steerAngle = steerAngle;
    }

    void ApplyBrakes()
    {
        float brake = brakeInput * brakeForce;

        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;
    }

    void ApplyAntiRoll(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit hit;
        float leftTravel = 1.0f;
        float rightTravel = 1.0f;

        bool leftGrounded = leftWheel.GetGroundHit(out hit);
        if (leftGrounded)
            leftTravel = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;

        bool rightGrounded = rightWheel.GetGroundHit(out hit);
        if (rightGrounded)
            rightTravel = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        float force = (leftTravel - rightTravel) * antiRollForce;

        if (leftGrounded)
            rb.AddForceAtPosition(leftWheel.transform.up * -force, leftWheel.transform.position);
        if (rightGrounded)
            rb.AddForceAtPosition(rightWheel.transform.up * force, rightWheel.transform.position);
    }

    void UpdateWheelVisuals()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftMesh);
        UpdateWheelPose(frontRightWheel, frontRightMesh);
        UpdateWheelPose(rearLeftWheel, rearLeftMesh);
        UpdateWheelPose(rearRightWheel, rearRightMesh);
    }

    void UpdateWheelPose(WheelCollider collider, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }

    void SetWheelFriction()
    {
        WheelCollider[] wheels = { frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel };
        foreach (WheelCollider wheel in wheels)
        {
            WheelFrictionCurve friction = wheel.sidewaysFriction;
            friction.extremumSlip = 0.2f;
            friction.extremumValue = 1f;
            friction.asymptoteSlip = 0.5f;
            friction.asymptoteValue = 0.75f;
            friction.stiffness = wheelStiffness;
            wheel.sidewaysFriction = friction;
        }
    }

    void UpdateSpeedUI()
    {
        if (speedText == null) return;
        float speedKmh = rb.linearVelocity.magnitude * 3.6f;
        speedText.text = Mathf.RoundToInt(speedKmh) + " KM/H";
    }
}

