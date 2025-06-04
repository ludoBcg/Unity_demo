using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100.0f;     // Rotation speed for A/D/W/S keys in degrees per second

    public GameObject player;
    private Player playerScript;
    private Vector3 viewDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        viewDir = Vector3.forward;

        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation with A/D keys
        float rotHorizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        //transform.Rotate(0, rotHorizontal, 0, Space.World); // Rotate around the global Y axis
        transform.RotateAround(playerScript.playerPos, Vector3.up, rotHorizontal);

        // Rotation with W/S keys
        float rotVertical = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        //transform.Rotate(-rotVertical, 0, 0, Space.Self); // Rotate around the global X axis
        if ((rotVertical < 0.0f && transform.rotation.eulerAngles.x > 80.0f && transform.rotation.eulerAngles.x < 278.0f) ||
             (rotVertical > 0.0f && transform.rotation.eulerAngles.x > 82.0f && transform.rotation.eulerAngles.x < 280.0f))
        {
            rotVertical = 0.0f;
        }

        transform.RotateAround(playerScript.playerPos, -transform.right, rotVertical);

        playerScript.rotateArrow(rotHorizontal, rotVertical);

        // send view direction to player, to be used as shooting direction
        viewDir = transform.forward;
        playerScript.setShootingDir(viewDir);
    }
}
