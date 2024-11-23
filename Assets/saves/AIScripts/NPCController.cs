using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public WaypointSystem waypointSystem;
    private NavMeshAgent agent;
    private NPCSpawner spawner;
    private bool isTony;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawner = FindObjectOfType<NPCSpawner>();
        isTony = (gameObject.name.ToLower() == "tony");

        if (waypointSystem == null)
        {
            Debug.LogError("WaypointSystem is not assigned to NPCController");
            return;
        }
        SetNewDestination();
    }

    void Update()
    {
        if (agent.remainingDistance < 0.1f && !agent.pathPending)
        {
            if (isTony)
            {
                SetNewDestination();
            }
            else if (!waypointSystem.IsWaypointVisible(transform))
            {
                DespawnNPC();
            }
            else
            {
                SetNewDestination();
            }
        }
    }

    void SetNewDestination()
    {
        if (waypointSystem == null) return;
        Transform newTarget = waypointSystem.GetRandomInvisibleWaypoint();
        if (newTarget != null && agent != null)
        {
            agent.SetDestination(newTarget.position);
        }
    }

    void DespawnNPC()
    {
        if (isTony) return; // Tony never despawns

        if (spawner != null)
        {
            spawner.DecreaseNPCCount();
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTony)
        {
            DespawnNPC();
        }
    }
}