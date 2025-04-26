using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    private Rigidbody carRb;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 driveDirection = carRb.transform.forward;
        float speed = carRb.mass * 4.25f;

        if(carRb.velocity.magnitude < 25.0f)
            carRb.AddForce(driveDirection * speed);

        // Destroy object when out-of-bounds
        if (transform.position.x < -100.0f || transform.position.x > 100.0f)
        {
            Destroy(gameObject);
        }
    }
}
