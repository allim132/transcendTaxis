using UnityEngine;
using System.Collections.Generic;

public class pasNPCSpawner : MonoBehaviour
{
    public GameObject interactableNPCPrefab;
    public pasWaypointSystem waypointSystem;
    public int maxNPCs = 5;
    public float spawnInterval = 5f;

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
        if (spawnPoint != null)
        {
            GameObject npcObject = Instantiate(interactableNPCPrefab, spawnPoint.position, Quaternion.identity);
            InteractableNPC npc = npcObject.GetComponent<InteractableNPC>();
            npc.Initialize(this);
            activeNPCs.Add(npc);
        }
    }

    public void RemoveNPC(InteractableNPC npc)
    {
        activeNPCs.Remove(npc);
    }
}