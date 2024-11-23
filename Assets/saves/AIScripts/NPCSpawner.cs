using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public WaypointSystem waypointSystem;
    public float spawnInterval = 5f;
    public int maxNPCs = 10;

    private float timer;
    private int currentNPCs;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && currentNPCs < maxNPCs)
        {
            SpawnNPC();
            timer = 0f;
        }
    }

    void SpawnNPC()
    {
        Transform spawnPoint = waypointSystem.GetRandomInvisibleWaypoint();
        if (spawnPoint != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPoint.position, out hit, 1.0f, NavMesh.AllAreas))
            {
                GameObject npc = Instantiate(npcPrefab, hit.position, Quaternion.identity);
                npc.GetComponent<NPCController>().waypointSystem = waypointSystem;
                currentNPCs++;
            }
        }
    }

    public void DecreaseNPCCount()
    {
        currentNPCs--;
    }
}