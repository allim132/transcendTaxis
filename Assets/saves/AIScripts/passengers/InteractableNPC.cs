using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    public float interactionRadius = 2f;
    public GameObject circlePrefab;
    private GameObject circleInstance;
    private pasNPCSpawner spawner;
    public Transform assignedDestination;
    public bool interacted = false;

    private float despawnInterval = 10f;
    private float timer;

    void Awake()
    {
        CreateCircle();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= despawnInterval && interacted)
        {
            Despawn();
            interacted = false;
            timer = 0f;
        }

        // Check for player in range
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player") && !GameManager.Instance.isPlayerOnTask)
            {
                OnInteract();
                break;
            }
        }
    }

    public void Initialize(pasNPCSpawner spawner, Transform destination)
    {
        this.spawner = spawner;
        this.assignedDestination = destination;
    }

    void CreateCircle()
    {
        if (circlePrefab != null)
        {
            circleInstance = Instantiate(circlePrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity);
            circleInstance.transform.localScale = Vector3.one * interactionRadius * 2;
            circleInstance.transform.SetParent(transform);
        }
        else
        {
            Debug.LogError("Circle prefab is not assigned to InteractableNPC");
        }
    }

    void OnInteract()
    {
        if (!interacted)
        {
            Debug.Log($"NPC Interacted! Destination: {assignedDestination.name}");
            interacted = true;
            GameManager.Instance.StartTask(assignedDestination);
            Despawn();
        }
    }

    void Despawn()
    {
        if (circleInstance != null)
        {
            Destroy(circleInstance);
        }
        spawner.RemoveNPC(this);
        Destroy(gameObject);
    }
}