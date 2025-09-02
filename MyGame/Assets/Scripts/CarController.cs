using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    // These public variables will show up in the Inspector, so we can tweak them live.
    public float moveSpeed = 50f;
    public float turnSpeed = 80f;

    private Rigidbody rb;
    private Vector2 moveInput;

    // Awake is called once when the script instance is being loaded.
    // It's the best place to get references to components.
    void Awake()
    {
        // Find the Rigidbody component that is on the same GameObject as this script.
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate.
    // All physics calculations (like forces and torques) should go here.
    void FixedUpdate()
    {
        // Add a force to the Rigidbody to move the car forward or backward.
        // We multiply by 'transform.forward' to make sure the force is always applied in the direction the car is facing.
        rb.AddForce(transform.forward * moveInput.y * moveSpeed);

        // Add a torque (rotational force) to the Rigidbody to turn the car.
        // We use 'Vector3.up' to rotate it around its vertical (Y) axis.
        rb.AddTorque(Vector3.up * moveInput.x * turnSpeed);
    }

    // This public method is specifically designed to be called by the Player Input component.
    // It receives the input data from our controls.
    public void OnMove(InputValue value)
    {
        // The input value is a Vector2 (an x and y value).
        // x corresponds to left/right (A/D keys) and y corresponds to forward/backward (W/S keys).
        moveInput = value.Get<Vector2>();
    }
}
