using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activator : MonoBehaviour
{
    public GameObject[] obj;
    public float activateRate = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
       
       /* for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(false);
        }
        StartCoroutine(switchOn());*/
    }
    IEnumerator switchOn() {
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(true);

            yield return new WaitForSeconds(activateRate);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
