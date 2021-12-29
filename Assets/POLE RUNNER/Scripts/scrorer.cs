using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class scrorer : MonoBehaviour
{
    public int num;
    Material clr1,clr2;
    public bool iAmLast = false;
    // Start is called before the first frame update
    void Start()
    {
        if (num <= 10)
        {
            clr1 = jayantManager.instance.mat[num].clr1;
            clr2 = jayantManager.instance.mat[num].clr2;
        }
        else {
            num = num / 10;
            clr1 = jayantManager.instance.mat[num].clr1;
            clr2 = jayantManager.instance.mat[num].clr2;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.name = num.ToString();
            transform.GetChild(i).GetChild(0).GetComponent<MeshRenderer>().material = clr1;
            transform.GetChild(i).GetChild(1).GetComponent<MeshRenderer>().material = clr2;
            transform.GetChild(i).GetChild(2).GetComponentInChildren<Text>().text ="X"+num;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (iAmLast&& jayantPlayer.instance.finishCrossed)
        {
            float diff = Vector3.Distance(transform.position, jayantPlayer.instance.gameObject.transform.position);
            if (diff<50)
            {
                Vector3 pos = transform.position;
                pos.z += 70;
                pos.y = -2;
                GameObject sc= Instantiate(jayantManager.instance.scrorer,pos,Quaternion.identity);
                sc.GetComponent<scrorer>().num = num + 1;
                sc.GetComponent<scrorer>().iAmLast = true;
                iAmLast = false;
            }
        }
    }
}
