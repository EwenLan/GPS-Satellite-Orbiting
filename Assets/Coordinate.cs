using UnityEngine;
using UnityEditor;

public class Coordinate
{
    static public readonly float earthSemiMajorAxis = 6378137.0f;
    static public readonly float earthEccentricity = 1f / 298.257223563f;
    static public readonly float earthRotationAngularVelocity = 7.2921151467e-5f;
    static public readonly float earthGM = 3.986005e14f;
    static public readonly float lightSpeed = 2.99792458e8f;
    static public readonly float Pi = 3.1415926535898f;

    static public Vector3 ECI2Unity(Vector3 ECICoordinate)
    {
        return new Vector3(ECICoordinate.x, ECICoordinate.z, ECICoordinate.y);
    }
    public static Vector3 Ellipse2ECI(Vector3 EclipseCoordinate, float semiMajorAxis, float Eccentricity, float perigeeArgument, float orbitInclinationAngle, float longitudeAscendingNode)
    {
        //float shortAxis = longAxis * Mathf.Sqrt(1 - Eccentricity * Eccentricity);
        float foci = semiMajorAxis * Eccentricity;
        Quaternion omegaRotation = Quaternion.Euler(new Vector3(0f, 0f, perigeeArgument * Mathf.Rad2Deg));
        Quaternion iRotation = Quaternion.Euler(new Vector3(orbitInclinationAngle * Mathf.Rad2Deg, 0f, 0f));
        Quaternion OmegaRotation = Quaternion.Euler(new Vector3(0f, 0f, longitudeAscendingNode * Mathf.Rad2Deg));
        return OmegaRotation * iRotation * omegaRotation * (new Vector3(EclipseCoordinate.x - foci, EclipseCoordinate.y, EclipseCoordinate.z));
    }
    public static float AngleNormalize(float angle, float lowerborder, float higherborder)
    {
        float angleRange = higherborder - lowerborder;
        while (angle > higherborder)
            angle -= angleRange;
        while (angle < lowerborder)
            angle += angleRange;
        return angle;
    }
}