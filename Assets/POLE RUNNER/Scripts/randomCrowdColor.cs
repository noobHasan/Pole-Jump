using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomCrowdColor : MonoBehaviour
{
    public MeshRenderer[] mr;
    private void Start()
    {
        int rc = Random.Range(0,jayantManager.instance.crowdMat.Length);
        Material c = jayantManager.instance.crowdMat[rc];
        for (int i = 0; i < mr.Length; i++)
        {
            mr[i].material = c;

        }
       /* for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }*/
       
    }
   private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("front"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

        }*/
        if (other.CompareTag("back"))
        {
            Destroy(gameObject);
        }
    }
}
