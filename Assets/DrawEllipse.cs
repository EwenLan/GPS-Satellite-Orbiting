using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawEllipse : MonoBehaviour
{
    LineRenderer lr;

    private readonly int segments = 64;
    public float semiMajorAxis = 4f;
    [Range(0f, 1f)]
    public float eccentricity = 0f;
    [Range(-Mathf.PI, Mathf.PI)]
    public float perigeeArgument = 0f;
    [Range(-Mathf.PI / 2, Mathf.PI / 2)]
    public float orbitInclinationAngle = 0f;
    [Range(0, 2 * Mathf.PI)]
    public float longitudeAscendingNode = 0f;
    private float yAxis = 1f;
    public void Awake()
    {
        lr = GetComponent<LineRenderer>();
        CalculateEllipse();
    }

    public void CalculateEllipse()
    {
        yAxis = semiMajorAxis * Mathf.Sqrt(1 - (eccentricity * eccentricity));
        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i < segments; ++i)
        {
            float angle = ((float)i / (float)segments) * 2f * Mathf.PI;
            float x = Mathf.Sin(angle) * semiMajorAxis;
            float y = Mathf.Cos(angle) * yAxis;
            points[i] = Coordinate.ECI2Unity(Coordinate.Ellipse2ECI(new Vector3(x, y, 0f), semiMajorAxis, eccentricity, perigeeArgument, orbitInclinationAngle, longitudeAscendingNode));
        }
        points[segments] = points[0];
        lr.positionCount = segments + 1;
        lr.SetPositions(points);
    }
    private void OnValidate()
    {
        if (Application.isPlaying && lr != null)
        {
            CalculateEllipse();
        }
    }
}
