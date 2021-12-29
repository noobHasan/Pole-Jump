using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copyPolePartPos : MonoBehaviour
{
    public GameObject target;
    public float lerpValue=3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,target.transform.position,Time.deltaTime* lerpValue);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, Time.deltaTime * lerpValue);
    }
}
