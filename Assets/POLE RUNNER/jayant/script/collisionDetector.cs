using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{
    public bool canCheckHurdle = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (!canCheckHurdle)
        {
            return;
        }
        if (other.CompareTag("hurdle"))
        {
            print("pole:hurdle");
           jayantPlayer.instance.stopRot = true;
            jayantPlayer.instance.destroyPole();
        }
        if (other.CompareTag("groundEnd"))
        {


            jayantPlayer.instance.pointerDownEvent();
        }
    }
}
