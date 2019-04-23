using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteGroupRotation : MonoBehaviour
{
    public SatelliteRotateAround motherSatellite;
    public Dropdown PRNDropdown;
    public Material SelectedSatelliteMaterial;
    public Material NormalSatelliteMaterial;
    private int currentSelectIndex = 0;
    private SatelliteRotateAround[] clonedSatellite;
    private readonly int satelliteNum = 24;
    private float[] longitudeAscendingNode;
    // Start is called before the first frame update
    void Start()
    {
        longitudeAscendingNode = new float[] { 1.038062244f, 2.085259795196598f,
        3.132457346393196f, 4.179654897589794f, 5.226852448786391f, -0.009135307196598f};
        PRNDropdown.ClearOptions();
        clonedSatellite = new SatelliteRotateAround[satelliteNum];
        for(int i = 0; i < satelliteNum; ++i)
        {
            int orbitPlane = i / 4;
            int prnInPlane = i % 4;
            clonedSatellite[i] = GameObject.Instantiate(motherSatellite);
            clonedSatellite[i].longitudeAscendingNode = longitudeAscendingNode[orbitPlane];
            clonedSatellite[i].toeMeanAnomaly += (orbitPlane * Mathf.PI / 6 + prnInPlane * Mathf.PI / 2);
            clonedSatellite[i].prn = i + 1;
            clonedSatellite[i].transform.parent = transform;
            ((Behaviour)clonedSatellite[i].GetComponent("Halo")).enabled = false;
            PRNDropdown.options.Add(new Dropdown.OptionData("PRN " + (i + 1).ToString("d2")));
        }
        clonedSatellite[17].displayArguments = true;
        PRNDropdown.value = 17;
        ((Behaviour)clonedSatellite[17].GetComponent("Halo")).enabled = true;
        motherSatellite.gameObject.SetActive(false);
        PRNDropdown.onValueChanged.AddListener(delegate { ChangePRNDisplay(); });
    }
    private void ChangePRNDisplay()
    {
        foreach(SatelliteRotateAround i in clonedSatellite)
        {
            i.displayArguments = false;
            i.gameObject.GetComponent<Renderer>().material = NormalSatelliteMaterial;
            ((Behaviour)i.GetComponent("Halo")).enabled = false;
        }
        clonedSatellite[PRNDropdown.value].displayArguments = true;
        clonedSatellite[PRNDropdown.value].gameObject.GetComponent<Renderer>().material = SelectedSatelliteMaterial;
        ((Behaviour)clonedSatellite[PRNDropdown.value].GetComponent("Halo")).enabled = true;
    }
}
