using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The taxi's transform
    public float distance = 10f;  // Distance from the target
    public float height = 10f;  // Height above the target
    public float smoothSpeed = 0.125f;  // How smoothly the camera follows the taxi

    private Vector3 offset;

    void Start()
    {
        // Calculate the initial offset
        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}