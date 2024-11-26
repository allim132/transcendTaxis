using UnityEngine;
using System.Collections.Generic;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance { get; private set; }

    public float interactionRadius = 2f;
    public GameObject circlePrefab;
    
    public NavigationArrowController arrowController;

    private List<Transform> destinations = new List<Transform>();
    private Dictionary<Transform, GameObject> destinationCircles = new Dictionary<Transform, GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Transform child in transform)
        {
            destinations.Add(child);
            CreateCircle(child);
        }
        HideAllDestinations();
    }

    void CreateCircle(Transform destination)
    {
        if (circlePrefab != null)
        {
            GameObject circleInstance = Instantiate(circlePrefab, destination.position + Vector3.up * 0.1f, Quaternion.identity);
            circleInstance.transform.localScale = Vector3.one * interactionRadius * 2;
            circleInstance.transform.SetParent(destination);
            destinationCircles[destination] = circleInstance;
        }
        else
        {
            Debug.LogError("Circle prefab is not assigned to DestinationManager");
        }
    }

    public Transform GetRandomDestination()
    {
        if (destinations.Count > 0)
        {
            return destinations[Random.Range(0, destinations.Count)];
        }
        return null;
    }

    public void ShowDestination(Transform destination)
    {
        HideAllDestinations();
        if (destinationCircles.TryGetValue(destination, out GameObject circle))
        {
            circle.SetActive(true);
        }
    }

    public void HideAllDestinations()
    {
        foreach (var circle in destinationCircles.Values)
        {
            circle.SetActive(false);
        }
    }

    void Update()
    {
        if (GameManager.Instance.isPlayerOnTask)
        {
            CheckPlayerReachedDestination();
        }
    }

    void CheckPlayerReachedDestination()
    {
        Transform currentDestination = GameManager.Instance.GetCurrentDestination();
        if (currentDestination != null)
        {
            Collider[] colliders = Physics.OverlapSphere(currentDestination.position, interactionRadius);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    OnDestinationReached(currentDestination);
                    break;
                }
            }
        }
    }

    void OnDestinationReached(Transform reachedDestination)
    {
        Debug.Log($"Player reached destination: {reachedDestination.name}");
        GameManager.Instance.CompleteTask();
        HideAllDestinations();

        arrowController.DropOffPassenger();
    }
}