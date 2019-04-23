using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewControl : MonoBehaviour
{
    private bool isRotating = false;
    public Transform worldCenter;
    private Vector3 offsetPosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(worldCenter.position);
        offsetPosition = transform.position - worldCenter.position;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * 16;
        if (Input.GetMouseButtonDown(0))
            isRotating = true;
        if (Input.GetMouseButtonUp(0))
            isRotating = false;
        if (isRotating)
        {
            Vector3 originalPosition = transform.position;
            Quaternion originalRotation = transform.rotation;
            transform.RotateAround(worldCenter.position, worldCenter.up, 6 * Input.GetAxis("Mouse X"));
            transform.RotateAround(worldCenter.position, transform.right, -6 * Input.GetAxis("Mouse Y"));
        }
        offsetPosition = transform.position - worldCenter.position;
    }
}
