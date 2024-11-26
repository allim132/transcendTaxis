using UnityEngine;

public class NavigationArrowController : MonoBehaviour
{
    public Transform car;
    public Transform pickupLocation;
    public Transform destination;
    // public GameManager theManager;
    public GameObject navigationArrow;

    public float arrowDistance = 3f; // Distance of arrow from car
    public float arrowHeight = 2f; // Height of arrow above car

    private bool hasPassenger = false;
    new Renderer renderer;
    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }
    void Update()
    {


        // Debug.Log("hasPassenger: " + hasPassenger);
        if (destination != null)
        {
            // Vector3 targetPosition = hasPassenger ? destination.position : pickupLocation.position;
            PointToDestination(destination.position);
        }

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
        destination = DestinationManager.Instance.GetActiveDestination();
        Debug.Log("Passenger Picked up");
        renderer.enabled = true;
        
    }

    public void DropOffPassenger()
    {
        hasPassenger = false;
        Debug.Log("Passenger Dropped off");
        renderer.enabled = false;
    }
}