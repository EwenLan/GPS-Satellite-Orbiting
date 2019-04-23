using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawEllipseGroup : MonoBehaviour
{
    public DrawEllipse motherEllipse;
    private DrawEllipse[] clonedEllipse;
    private int orbitNum = 6;
    // Start is called before the first frame update
    void Start()
    {
        //clonedEllipse = GameObject.Instantiate(motherEllipse);
        clonedEllipse = new DrawEllipse[orbitNum];
        for (int i = 0; i < orbitNum; ++i)
        {
            clonedEllipse[i] = GameObject.Instantiate(motherEllipse);
            clonedEllipse[i].gameObject.SetActive(true);
            clonedEllipse[i].transform.parent = transform;
            clonedEllipse[i].semiMajorAxis = 4.168915877302024f;
            clonedEllipse[i].eccentricity = 0.005912038265f;
            clonedEllipse[i].perigeeArgument = -1.717457876f;
            clonedEllipse[i].orbitInclinationAngle = 0.9848407f;
        }
        clonedEllipse[0].longitudeAscendingNode = -0.009135307196598f;
        clonedEllipse[1].longitudeAscendingNode = 1.038062244f;
        clonedEllipse[2].longitudeAscendingNode = 2.085259795196598f;
        clonedEllipse[3].longitudeAscendingNode = 3.132457346393196f;
        clonedEllipse[4].longitudeAscendingNode = 4.179654897589794f;
        clonedEllipse[5].longitudeAscendingNode = 5.226852448786391f;
        for (int i = 0; i < orbitNum; ++i)
        {
            clonedEllipse[i].CalculateEllipse();
        }
        motherEllipse.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
