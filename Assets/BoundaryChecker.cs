using UnityEngine;

/*
 * Lead Developer Note:
 * This code is to account for if the car goes out of boundaries (whether that be driving off the map or getting launched from the map).
 */
public class BoundaryChecker : MonoBehaviour
{
    public float minX, maxX, minY, maxY, minZ, maxZ;
    private Transform taxiTransform;

    void Start()
    {
        taxiTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 position = taxiTransform.position;
        bool isOutOfBounds = position.x < minX || position.x > maxX ||
                             position.y < minY || position.y > maxY ||
                             position.z < minZ || position.z > maxZ;

        if (isOutOfBounds)
        {
            HandleOutOfBounds();
        }
    }

    void HandleOutOfBounds()
    {
        // Reset the taxi's position or implement game over logic
        taxiTransform.position = new Vector3(-80f, 1f, 12f); // Example reset position
        Debug.Log("Taxi out of bounds!");
    }
}