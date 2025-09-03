using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public class CarAudio : MonoBehaviour
{
    public AudioClip engineSound;
    [Range(0.5f, 2.5f)] public float minPitch = 0.8f;
    [Range(1.5f, 4.5f)] public float maxPitch = 2.5f;
    [Range(10f, 100f)] public float maxSpeedForPitch = 50f;

    private AudioSource audioSource;
    private Rigidbody rb;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        audioSource.clip = engineSound;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 1.0f; // 3D sound
        audioSource.Play();
    }

    void Update()
    {
        UpdateEngineSound();
    }

    void UpdateEngineSound()
    {
        if (rb == null || audioSource == null) return;
        float speed = rb.linearVelocity.magnitude;
        float pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeedForPitch);
        audioSource.pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    // --- NEW PUBLIC METHODS ---
    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void ResumeSound()
    {
        audioSource.UnPause();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}

