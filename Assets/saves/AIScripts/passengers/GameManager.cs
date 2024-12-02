using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isPlayerOnTask { get; private set; }
    private Transform currentDestination;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartTask(Transform destination)
    {
        if (destination == null)
        {
            Debug.LogError("StartTask called with a null destination");
            return;
        }

        isPlayerOnTask = true;
        currentDestination = destination;
        DestinationManager.Instance.ShowDestination(destination);
    }

    public void CompleteTask()
    {
        isPlayerOnTask = false;
        currentDestination = null;
        DestinationManager.Instance.HideAllDestinations();
    }

    public bool IsCurrentDestination(Transform destination)
    {
        // Debug.Log("GetCurrentDestination called. Current destination: " + (currentDestination != null ? currentDestination.name : "None"));

        return destination == currentDestination;
    }

    public Transform GetCurrentDestination()
    {
        // Debug.Log("GameManager's GetCurrentDestination called");
        return currentDestination;
    }
}