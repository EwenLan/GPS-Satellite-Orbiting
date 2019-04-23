using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteRotateAround : MonoBehaviour
{
    private bool orbitActive = true;
    public God god;

    private float toe = 244800f;
    private float sqrtSemiMajorAxis = 5153.65531f;
    [Range(0f, 1f)]
    private float eccentricity = 0.005912038265f;
    [Range(-Mathf.PI, Mathf.PI)]
    private float orbitInclinationAngle = 0.9848407943f;
    [Range(0, 2 * Mathf.PI)]
    public float longitudeAscendingNode = 1.038062244f;
    //private float longitudeAscendingNode = 0f;
    [Range(-Mathf.PI, Mathf.PI)]
    private float perigeeArgument = -1.717457876f;
    [Range(0f, 2 * Mathf.PI)]
    public float toeMeanAnomaly = -1.064739758f;
    private float deltaN = 4.249105564e-9f;
    private float iDot = 7.422851197e-51f;
    private float OmegaDot = -8.151768125e-9f;
    private float cuc = 3.054738045e-7f;
    private float cus = 2.237036824e-6f;
    private float crc = 350.53125f;
    private float crs = 2.53125f;
    private float cic = -8.381903172e-8f;
    private float cis = 8.940696716e-8f;

    public int prn = 0;
    public float orbitPeroid = 0f;
    public float semiMajorAxis = 0f;
    public float yAxis = 0f;
    public float angularVelocity = 0f;
    public float accurateAngularVelocity = 0f;
    public float meanAnomaly = 0f;
    public float eccentricAnomaly = 0f;
    public float trueAnomaly = 0f;
    public float ascendingNodeArgument = 0f;
    public float distance = 0f;
    public float deltaUk = 0f;
    public float deltaRk = 0f;
    public float deltaIk = 0f;
    public float correctedAscendingNodeArgument = 0f;
    public float correctedDistance = 0f;
    public float correctedOrbitInclinationAngle = 0f;
    public float orbitPlaneX = 0f;
    public float orbitPlaneY = 0f;
    public float tLongitudeAscendingNode = 0f;
    public float ECEFx = 0f;
    public float ECEFy = 0f;
    public float ECEFz = 0f;
    public bool displayArguments = false;
    public Text PRNText;
    public Text WGS84X;
    public Text WGS84Y;
    public Text WGS84Z;
    public Text GCSLat;
    public Text GCSLon;
    public Text GCSHeight;

    public float plotSemiMajorAxis = 0f;
    public float plotSemiMinorAxis = 0f;
    public float plotDistance = 0f;
    public float ellipseX = 0f;
    public float ellipseY = 0f;
    public bool isPlotOrbit = false;

    private void Awake()
    {
        CalculateSatelliteOrbit();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateOrbit());
    }

    private void OnValidate()
    {
        
    }
    private void CalculateSatelliteOrbit()
    {
        semiMajorAxis = Mathf.Pow(sqrtSemiMajorAxis, 2);
        yAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
        orbitPeroid = Mathf.Sqrt(4 * Mathf.Pow(Mathf.PI, 2) * Mathf.Pow(semiMajorAxis, 3) / Coordinate.earthGM);
        angularVelocity = 2 * Mathf.PI / orbitPeroid;
        accurateAngularVelocity = angularVelocity + deltaN;
        orbitPeroid = 2 * Mathf.PI / accurateAngularVelocity;
        plotSemiMajorAxis = semiMajorAxis / Coordinate.earthSemiMajorAxis;
        plotSemiMinorAxis = yAxis / Coordinate.earthSemiMajorAxis;
    }
    IEnumerator AnimateOrbit()
    {
        CalculateSatelliteOrbit();
        while (orbitActive)
        {
            // Step 1
            float tk = 0f;
            if (god != null)
            {
                tk = god.currentTime - toe;
            }

            // Step 2
            // Step 3
            meanAnomaly = toeMeanAnomaly + accurateAngularVelocity * tk;
            meanAnomaly = Coordinate.AngleNormalize(meanAnomaly, 0, 2 * Mathf.PI);
            // Step 4
            for (int i = 0; i < 5; ++i)
            {
                eccentricAnomaly = meanAnomaly + eccentricity * Mathf.Sin(eccentricAnomaly);
            }
            // Step 5
            distance = semiMajorAxis * (1 - eccentricity * Mathf.Cos(eccentricAnomaly));
            float sinTrueAnomaly = 0f;
            sinTrueAnomaly = Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2)) * Mathf.Sin(eccentricAnomaly) / (1 - eccentricity * Mathf.Cos(eccentricAnomaly));
            trueAnomaly = Mathf.Atan(Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2)) * Mathf.Sin(eccentricAnomaly) / (Mathf.Cos(eccentricAnomaly) - eccentricity));
            if ((trueAnomaly > 0 && sinTrueAnomaly < 0) || (trueAnomaly < 0 && sinTrueAnomaly > 0))
                trueAnomaly += Mathf.PI;
            trueAnomaly = Coordinate.AngleNormalize(trueAnomaly, -Mathf.PI, Mathf.PI);
            // Step 6
            ascendingNodeArgument = trueAnomaly + perigeeArgument;
            // Step 7
            deltaUk = cus * Mathf.Sin(2 * ascendingNodeArgument) + cuc * Mathf.Cos(2 * ascendingNodeArgument);
            deltaRk = crs * Mathf.Sin(2 * ascendingNodeArgument) + crc * Mathf.Cos(2 * ascendingNodeArgument);
            deltaIk = cis * Mathf.Sin(2 * ascendingNodeArgument) + cic * Mathf.Cos(2 * ascendingNodeArgument);
            // Step 8
            //ascendingNodeArgument = ascendingNodeArgument + deltaUk;
            correctedAscendingNodeArgument = ascendingNodeArgument + deltaUk;
            //distance = semiMajorAxis * (1 - eccentricity * Mathf.Cos(eccentricAnomaly)) + deltaRk;
            correctedDistance = semiMajorAxis * (1 - eccentricity * Mathf.Cos(eccentricAnomaly)) + deltaRk;
            //orbitInclinationAngle = orbitInclinationAngle + iDot * tk + deltaIk;
            correctedOrbitInclinationAngle = orbitInclinationAngle + iDot * tk + deltaIk;
            correctedOrbitInclinationAngle = Coordinate.AngleNormalize(correctedOrbitInclinationAngle, -Mathf.PI, Mathf.PI);
            // Step 9
            orbitPlaneX = correctedDistance * Mathf.Cos(correctedAscendingNodeArgument);
            orbitPlaneY = correctedDistance * Mathf.Sin(correctedAscendingNodeArgument);
            // Step 10
            tLongitudeAscendingNode = longitudeAscendingNode + (OmegaDot - Coordinate.earthRotationAngularVelocity) * tk - Coordinate.earthRotationAngularVelocity * toe;
            tLongitudeAscendingNode = Coordinate.AngleNormalize(tLongitudeAscendingNode, 0, 2 * Mathf.PI);
            // Step 11
            ECEFx = orbitPlaneX * Mathf.Cos(tLongitudeAscendingNode) - orbitPlaneY * Mathf.Cos(correctedOrbitInclinationAngle) * Mathf.Sin(tLongitudeAscendingNode);
            ECEFy = orbitPlaneX * Mathf.Sin(tLongitudeAscendingNode) + orbitPlaneY * Mathf.Cos(correctedOrbitInclinationAngle) * Mathf.Cos(tLongitudeAscendingNode);
            ECEFz = orbitPlaneY * Mathf.Sin(orbitInclinationAngle);

            plotDistance = plotSemiMajorAxis * (1 - eccentricity * Mathf.Cos(eccentricAnomaly));
            ellipseX = plotDistance * Mathf.Cos(trueAnomaly) + plotSemiMajorAxis * eccentricity;
            ellipseY = plotDistance * Mathf.Sin(trueAnomaly);
            //Vector3 satellitePosition = Coordinate.ECI2Unity(Coordinate.Ecllipse2ECI(new Vector3(ellipseX, ellipseX, 0f), plotSemiMajorAxis, eccentricity, perigeeArgument, orbitInclinationAngle, longitudeAscendingNode));
            Vector3 satellitePosition = Coordinate.ECI2Unity(Coordinate.Ellipse2ECI(new Vector3(ellipseX, ellipseY, 0f), plotSemiMajorAxis, eccentricity, perigeeArgument, orbitInclinationAngle, longitudeAscendingNode));
            //Vector3 satellitePosition = (new Vector3(ECEFx, ECEFy, ECEFz)) / Coordinate.earthSemiMajorAxis;
            transform.localPosition = satellitePosition;
            if (displayArguments)
            {
                PRNText.text = prn.ToString("D2");
                WGS84X.text = ECEFx.ToString("e");
                WGS84Y.text = ECEFy.ToString("e");
                WGS84Z.text = ECEFz.ToString("e");
                float p = Mathf.Sqrt(Mathf.Pow(ECEFx, 2) + Mathf.Pow(ECEFx, 2));
                float lon = Mathf.Atan(ECEFy / ECEFx);
                if (ECEFy > 0 && lon < 0)
                {
                    lon += Mathf.PI;
                }
                else
                {
                    if (yAxis < 0 && lon > 0)
                    {
                        lon -= Mathf.PI;
                    }
                }
                float height = 0f;
                float lat = 0f;
                float N = 0;
                for (int i = 0; i < 6; ++i)
                {
                    height = p / Mathf.Cos(lat) - N;
                    N = Coordinate.earthSemiMajorAxis / Mathf.Sqrt(1 - Mathf.Pow(Coordinate.earthEccentricity, 2) * Mathf.Pow(Mathf.Sin(lat), 2));
                    lat = Mathf.Atan(ECEFz / p * (1 - Mathf.Pow(Coordinate.earthEccentricity, 2) * N / (N + height)));
                }
                GCSLat.text = (lat * Mathf.Rad2Deg).ToString("f6");
                GCSLon.text = (lon * Mathf.Rad2Deg).ToString("f6");
                GCSHeight.text = height.ToString("f0");
            }
            yield return null;
        }
    }
}
