using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clrChangeScript : MonoBehaviour
{
    public Material changeMat;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.color = changeMat.color;
        transform.GetChild(1).GetComponent<ParticleSystem>().startColor = changeMat.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
