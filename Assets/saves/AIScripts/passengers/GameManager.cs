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
        return destination == currentDestination;
    }

    public Transform GetCurrentDestination()
    {
        return currentDestination;
    }
}