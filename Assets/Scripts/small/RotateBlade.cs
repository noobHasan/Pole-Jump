using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlade : MonoBehaviour
{
    public Vector3 rot;
    public float speed = 60f;
    void Update()
    {
        transform.Rotate(rot* Time.deltaTime * speed);
    }
}
