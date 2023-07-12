using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierLine : MonoBehaviour
{
    Vector3 p0;
    Vector3 p1;
    Vector3 p2;
    Vector3 p3;

    public int pointCount;

    public float curve;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        curve = 0;
    }

    public void UpdateBezierLine()
    {
        Vector3[] points = new Vector3[pointCount];
        p0 = lineRenderer.GetPosition(0);
        p3 = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        curve = Mathf.Abs((p0.x - p3.x) * 0.5f);
        if (curve > 100f)
        {
            curve = 100f;
        }
        p1 = new Vector3(p0.x + curve, p0.y, p0.z);
        p2 = new Vector3(p3.x - curve, p3.y, p3.z);

        for (int i = 0; i < pointCount; i++)
            points[i] = GetBezierPosition(p0, p1, p2, p3, (float)i / (pointCount - 1));

        lineRenderer.positionCount = pointCount;
        lineRenderer.SetPositions(points);
    }

    private Vector3 GetBezierPosition(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 q0 = Vector3.Lerp(p0, p1, t);
        Vector3 q1 = Vector3.Lerp(p1, p2, t);
        Vector3 q2 = Vector3.Lerp(p2, p3, t);

        Vector3 r0 = Vector3.Lerp(q0, q1, t);
        Vector3 r1 = Vector3.Lerp(q1, q2, t);

        return Vector3.Lerp(r0, r1, t);
    }
}