using UnityEngine;

public class Package : MonoBehaviour
{
    // How fast the package spins
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the package around the Y-axis (up)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    // This function is called by Unity when another collider with a Rigidbody enters it
    private void OnTriggerEnter(Collider other)
    {
        // We check if the object that hit us has the "Player" tag
        if (other.CompareTag("Player"))
        {
            Debug.Log("Package picked up by player!");

            // TODO: Tell the GameManager to find a new location

            // Destroy the package object
            Destroy(gameObject);
        }
    }
}
