using UnityEngine;

public class TaxiController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody rb; // Object is solid

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        MoveTaxi();
    }

    void HandleInput()
    {
        if (Input.touchCount > 0) // Touch input 
        {
            Touch touch = Input.GetTouch(0);
            HandleInputPosition(touch.position);
        }
        else if (Input.GetMouseButton(0)) // Mouse input for testing
        {
            HandleInputPosition(Input.mousePosition);
        }
    }

    void HandleInputPosition(Vector2 inputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Ensure the target is at the same Y level as the taxi
            targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            isMoving = true;
        }
    }

    void MoveTaxi()
    {
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

            Vector3 movement = transform.forward * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            if (Vector3.Distance(rb.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}