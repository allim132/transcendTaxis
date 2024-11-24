using UnityEngine;
using System.Collections.Generic;

public class pasWaypointSystem : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private List<Transform> availableWaypoints = new List<Transform>();

    void Awake()
    {
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
        ResetAvailableWaypoints();
    }

    public Transform GetRandomAvailableWaypoint()
    {
        if (availableWaypoints.Count == 0)
        {
            ResetAvailableWaypoints();
        }
        int index = Random.Range(0, availableWaypoints.Count);
        Transform waypoint = availableWaypoints[index];
        availableWaypoints.RemoveAt(index);
        return waypoint;
    }

    private void ResetAvailableWaypoints()
    {
        availableWaypoints.Clear();
        availableWaypoints.AddRange(waypoints);
    }
}