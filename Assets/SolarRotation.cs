using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarRotation : MonoBehaviour
{
    private float radius = 10f;
    private float eccentricity = 0f;
    private float orbitInclinationAngle = 0.409092629495478f;
    private float longitudeAscendingNode = 0f;
    private float perigeeArgument = 0f;
    private float orbitPeroid = 3.155814976354560e7f;
    private bool orbitActive = true;

    public God god;

    public float trueAnomaly = 0f;
    public float ellipseX = 10f;
    public float ellipseY = 0f;
    public float pitchAngle = 0f;

    void Start()
    {
        StartCoroutine(SolarRotateAround());
    }
    IEnumerator SolarRotateAround()
    {
        while (orbitActive)
        {
            float progress = 0f;
            if (god != null)
            {
                progress = god.currentTime / orbitPeroid;
            }
            while (progress > 1)
            {
                progress -= 1;
            }
            trueAnomaly = progress * 2 * Mathf.PI;
            ellipseX = radius * Mathf.Cos(trueAnomaly);
            ellipseY = radius * Mathf.Sin(trueAnomaly);
            Vector3 solarPosition = Coordinate.ECI2Unity(Coordinate.Ellipse2ECI(new Vector3(ellipseX, ellipseY, 0f), radius, eccentricity, perigeeArgument, orbitInclinationAngle, longitudeAscendingNode));
            transform.position = solarPosition;
            pitchAngle = Mathf.Asin(solarPosition.y / radius);
            Quaternion solarLightRotation = Quaternion.Euler(pitchAngle * Mathf.Rad2Deg, -90 - trueAnomaly * Mathf.Rad2Deg, 0f);
            transform.rotation = solarLightRotation;
            yield return null;
        }
    }
}
