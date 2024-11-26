using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private int segments = 60;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        CreateCircle();
    }

    void CreateCircle()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments + 1;

        float deltaTheta = (2f * Mathf.PI) / segments;
        float theta = 0f;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = 0.5f * Mathf.Cos(theta);
            float z = 0.5f * Mathf.Sin(theta);
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            theta += deltaTheta;
        }
    }
}