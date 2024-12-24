using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FanRotation : MonoBehaviour
{
    // Speed of rotation
    public float rotationSpeed = 0f;

    // Maximum speed
    public float maxRotationSpeed = 100f;

    // Time to reach max speed
    public float accelerationTime = 3f;

    // Flag to indicate if rotation should start
    public bool rotateStarted = false;

    // Update is called once per frame
    void Update()
    {
        // Check if Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            rotateStarted = true;
        }

        if (rotateStarted)
        {
            // Accelerate the rotation speed
            rotationSpeed = Mathf.Lerp(rotationSpeed, maxRotationSpeed, Time.deltaTime / accelerationTime);

            // Rotate the fan on the X-axis
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
    }
}
