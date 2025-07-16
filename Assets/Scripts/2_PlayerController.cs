using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // forward speed
    [SerializeField] float horsePower = 15000.0f;
    // turning speed
    [SerializeField] float turnSpeed = 5000.0f;
    // maximum speed
    [SerializeField] float maxSpeed = 30.0f;

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
        //playerRB.centerOfMass = centerOfMass.transform.localPosition;
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

        // rotate vehicle
        playerRB.AddTorque(playerRB.transform.up * turnSpeed * horizontalInput);

        foreach (WheelCollider wheelCollider in allWheels)
        {
            if (!wheelCollider.isGrounded)
            {
                // skip update
                return;
            }
        }


        // Move vehicle forward
        //transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        if (speed < maxSpeed)
            playerRB.AddForce(playerRB.transform.forward * horsePower * forwardInput);


    }

}
