using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform playerTarget; // The player's car

    private float cameraHeight; // We'll store the camera's starting height

    void Start()
    {
        // Store the initial Y position (height) of the minimap camera
        if (playerTarget != null)
        {
            cameraHeight = transform.position.y;
        }
    }

    void LateUpdate()
    {
        if (playerTarget == null)
        {
            return; // Don't do anything if we have no target
        }

        // --- Camera Position Logic (Same as before) ---
        // The camera's position is updated to follow the player from above.
        Vector3 newPosition = new Vector3(playerTarget.position.x, cameraHeight, playerTarget.position.z);
        transform.position = newPosition;

        // --- NEW: Camera Rotation Logic ---
        // We create a new rotation based on the player's current Y-axis rotation.
        // We keep the X-axis at 90 degrees to always look straight down.
        Quaternion newRotation = Quaternion.Euler(90f, playerTarget.eulerAngles.y, 0f);
        transform.rotation = newRotation;
    }
}