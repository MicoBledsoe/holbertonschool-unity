using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0); // This will rotate the object around its Y axis

    void Update()
    {
        // Rotate the object every frame based on the given rotationSpeed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
