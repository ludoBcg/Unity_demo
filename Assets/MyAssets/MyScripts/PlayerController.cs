using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // forward speed
    [SerializeField] float horsePower = 10.0f;
    // turning speed
    [SerializeField] float turnSpeed = 45.0f;

    // display speed
    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] float speed;
    [SerializeField] TextMeshProUGUI rpmText;
    [SerializeField] float rpm;

    // wheel colliders
    [Header("WheelCollider")]
    [SerializeField] List<WheelCollider> allWheels;
    [SerializeField] int wheelsOnGround;

    // rigid body
    private Rigidbody playerRB;
    [SerializeField] GameObject centerOfMass;

    private float horizontalInput;
    private float forwardInput;


    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerRB.centerOfMass = centerOfMass.transform.position;
    }

    void Update()
    {
        speed = Mathf.Round(playerRB.velocity.magnitude * 2.237f); //3.6 for kph
        speedometerText.SetText("Speed: " + speed + "mph");

        rpm = Mathf.Round((speed % 30) * 40);
        rpmText.SetText("RPM: " + rpm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // left/right user controls
        horizontalInput = Input.GetAxis("Horizontal");
        // frontward/backward user controls
        forwardInput = Input.GetAxis("Vertical");

        // Move vehicle forward
        //transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        playerRB.AddRelativeForce(transform.forward * horsePower * forwardInput);

        // rotate vehicle
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
    }
}
