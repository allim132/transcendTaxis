using UnityEngine;

public class TaxiController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float maxBreakingForce;
    public float brakingRate;
    public float acceleration;
    public float deceleration;
    public float collisionForce = 10f; // Force to apply when colliding
    public float collisionCheckDistance = 0.5f; // Distance to check for collisions
    public float bounciness = 0.5f; // Adjust this value in the Inspector

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Rigidbody rb;
    private float currentSpeed = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MoveTaxi();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector2 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
            HandleInputPosition(inputPosition);
        } else
        {
            isMoving = false;
        }
    }

    void HandleInputPosition(Vector2 inputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            isMoving = true;
        }
    }

    private void MoveTaxi()
    {
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.fixedDeltaTime);
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;

            // Check for collision before moving
            if (!CheckCollision(movement))
            {
                rb.MovePosition(rb.position + movement);
            }

            if (Vector3.Distance(rb.position, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;

            // Check for collision before moving
            if (!CheckCollision(movement))
            {
                rb.MovePosition(rb.position + movement);
            }
        }
    }

    private bool CheckCollision(Vector3 movement)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, movement.normalized, out hit, movement.magnitude + collisionCheckDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                // Calculate the angle between the car's forward direction and the wall's normal
                float impactAngle = Vector3.Angle(transform.forward, hit.normal);

                // Determine the severity of the collision based on the impact angle
                float collisionSeverity = Mathf.Clamp01(1 - (impactAngle / 90f) * bounciness);

                // Calculate a reduced speed based on the collision severity
                float reducedSpeed = currentSpeed * (1 - collisionSeverity);

                // Calculate a blended direction between the current forward and the reflection
                Vector3 reflectDir = Vector3.Reflect(movement.normalized, hit.normal);
                Vector3 blendedDir = Vector3.Lerp(transform.forward, reflectDir, collisionSeverity);

                // Apply the new velocity
                rb.linearVelocity = blendedDir * reducedSpeed;

                // Update current speed
                currentSpeed = reducedSpeed;

                // Smoothly rotate towards the new direction
                Quaternion targetRotation = Quaternion.LookRotation(blendedDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, collisionSeverity);

                // Apply a small backwards force to simulate impact
                rb.AddForce(-movement.normalized * collisionForce, ForceMode.Impulse);

                return true; // Collision detected
            }
        }
        return false; // No collision
    }


}