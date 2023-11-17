using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//My directives above ^

public class FORCERESPAWN : MonoBehaviour
{
    public float THRESHOLD;

    void FixedUpdate()
    {
        if(transform.position.y < THRESHOLD) //if the OGPlayer goes below a certain height which in the inspector i set to -9
        {
            transform.position = new Vector3(0f, 0.358517f, 0f); //the postion where the OGPlayer started
        }
    }
}
