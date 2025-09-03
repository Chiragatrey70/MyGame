using UnityEngine;

public class DropOff : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Package Delivered!");

            // TODO: Tell GameManager to add time and spawn a new package
            GameManager.Instance.OnPackageDelivered();

            // Deactivate the drop-off zone
            Destroy(gameObject);
        }
    }
}
