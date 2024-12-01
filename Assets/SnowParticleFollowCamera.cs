using UnityEngine;

public class SnowParticleFollowCamera : MonoBehaviour
{
    public Camera mainCamera;
    public float heightOffset = 10f;
    public float forwardOffset = 5f;

    private ParticleSystem rainParticleSystem;

    void Start()
    {
        // Get the ParticleSystem component attached to this GameObject
        rainParticleSystem = GetComponent<ParticleSystem>();

        // If no camera is assigned, try to find the main camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Ensure we have both a particle system and a camera
        if (rainParticleSystem == null || mainCamera == null)
        {
            Debug.LogError("Rain Particle System or Main Camera not found!");
            enabled = false;
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Calculate the new position for the particle system
            Vector3 newPosition = mainCamera.transform.position +
                                  (mainCamera.transform.up * heightOffset) +
                                  (mainCamera.transform.forward * forwardOffset);

            // Update the particle system's position
            transform.position = newPosition;
        }
    }
}