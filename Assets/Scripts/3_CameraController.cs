using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100.0f;     // Rotation speed for A/D/W/S keys in degrees per second

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation with A/D keys
        float rotHorizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotHorizontal, 0, Space.World); // Rotate around the global Y axis

        // Rotation with W/S keys
        float rotVertical = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        transform.Rotate(-rotVertical, 0, 0, Space.World); // Rotate around the global X axis
    }
}
