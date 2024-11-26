using UnityEngine;
using System.Collections.Generic;

public class pasNPCSpawner : MonoBehaviour
{
    public GameObject interactableNPCPrefab;
    public pasWaypointSystem waypointSystem;
    public DestinationManager destinationManager;
    public int maxNPCs = 1;
    public float spawnInterval = 10f;

    private List<InteractableNPC> activeNPCs = new List<InteractableNPC>();
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && activeNPCs.Count < maxNPCs)
        {
            SpawnNPC();
            timer = 0f;
        }
    }

    void SpawnNPC()
    {
        Transform spawnPoint = waypointSystem.GetRandomAvailableWaypoint();
        if (spawnPoint != null && destinationManager != null)
        {
            Transform randomDestination = destinationManager.GetRandomDestination();
            if (randomDestination != null)
            {
                GameObject npcObject = Instantiate(interactableNPCPrefab, spawnPoint.position, Quaternion.identity);
                InteractableNPC npc = npcObject.GetComponent<InteractableNPC>();
                npc.Initialize(this, randomDestination);
                activeNPCs.Add(npc);
            }
            else
            {
                Debug.LogError("No destinations available in DestinationManager");
            }
        }
        else
        {
            Debug.LogError("Spawn point or DestinationManager is not available");
        }
    }

    public void RemoveNPC(InteractableNPC npc)
    {
        activeNPCs.Remove(npc);
    }
}