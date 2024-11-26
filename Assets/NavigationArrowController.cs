using UnityEngine;

public class NavigationArrowController : MonoBehaviour
{
    public Transform car;
    public Transform pickupLocation;
    public Transform destination;
    public GameObject navigationArrow;

    public float arrowDistance = 3f; // Distance of arrow from car
    public float arrowHeight = 2f; // Height of arrow above car

    private bool hasPassenger = false;

    void Update()
    {
        Vector3 targetPosition = hasPassenger ? destination.position : pickupLocation.position;
        PointToDestination(targetPosition);
    }

    void PointToDestination(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - car.position;
        directionToTarget.y = 0; // Keep arrow horizontal

        // Calculate arrow position
        Vector3 arrowPosition = car.position + Vector3.up * arrowHeight;
        arrowPosition += Vector3.ProjectOnPlane(directionToTarget.normalized, Vector3.up) * arrowDistance;

        // Set arrow position and rotation
        navigationArrow.transform.position = arrowPosition;
        navigationArrow.transform.rotation = Quaternion.LookRotation(directionToTarget);
    }

    public void PickupPassenger()
    {
        hasPassenger = true;
    }

    public void DropOffPassenger()
    {
        hasPassenger = false;
    }
}