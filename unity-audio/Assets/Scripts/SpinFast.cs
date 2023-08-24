using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinFast : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 200, 0); // Increased Y-axis rotation speed for faster spinning

    void Update()
    {
        // Rotate the object every frame based on the increased rotationSpeed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
