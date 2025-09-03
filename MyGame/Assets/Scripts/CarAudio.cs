using UnityEngine;

// This ensures the necessary components are on the car.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class CarAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip engineSound;
    [Range(0.5f, 2.5f)] public float minPitch = 0.8f;
    [Range(1.0f, 4.0f)] public float maxPitch = 2.5f;
    [Range(50f, 200f)] public float maxSpeedForPitch = 150f; // The speed (in KM/H) at which the engine reaches max pitch

    private Rigidbody rb;
    private AudioSource audioSource;

    void Start()
    {
        // Get the components from the car
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Set up the audio source to play our engine sound
        audioSource.clip = engineSound;
        audioSource.loop = true;
        audioSource.spatialBlend = 1.0f; // Make it 3D sound
        audioSource.Play();
    }

    void Update()
    {
        // This function runs every frame to update the sound's pitch
        UpdateEngineSound();
    }

    void UpdateEngineSound()
    {
        if (engineSound == null) return; // Don't do anything if no sound is assigned

        // Calculate current speed as a fraction of the max speed
        // (rb.linearVelocity.magnitude * 3.6f) converts the speed to KM/H --- THIS LINE IS UPDATED
        float speedFraction = Mathf.Clamp01((rb.linearVelocity.magnitude * 3.6f) / maxSpeedForPitch);

        // Smoothly adjust the pitch between our min and max values based on the car's speed
        audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedFraction);
    }
}

