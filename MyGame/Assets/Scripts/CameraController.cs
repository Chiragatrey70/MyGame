using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The car to follow
    public Vector3 offset = new Vector3(0, 8f, -15f); // The distance behind and above the car. Tweak these values!

    // LateUpdate is called every frame, after all other Update functions have been called.
    // This is the best place for camera logic, as it ensures the target has already moved for the frame.
    void LateUpdate()
    {
        if (target != null)
        {
            // Set the camera's position to be the target's position plus our offset.
            transform.position = target.position + offset;

            // Make the camera look directly at the target's position.
            transform.LookAt(target);
        }
    }
}
