using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 1.0f;
    public float length = 10.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newPos = Mathf.PingPong(Time.time * speed, length) - length / 2;
        transform.position = startPos + new Vector3(newPos, 0, 0);
    }
}
