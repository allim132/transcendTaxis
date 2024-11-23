using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WaypointSystem : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    void Awake()
    {
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }
    }

    public Transform GetRandomInvisibleWaypoint()
    {
        List<Transform> invisibleWaypoints = waypoints.Where(w => !IsWaypointVisible(w)).ToList();
        if (invisibleWaypoints.Count > 0)
        {
            return invisibleWaypoints[Random.Range(0, invisibleWaypoints.Count)];
        }
        return null;
    }

    public bool IsWaypointVisible(Transform waypoint)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(waypoint.position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
               viewportPoint.z > 0;
    }
}