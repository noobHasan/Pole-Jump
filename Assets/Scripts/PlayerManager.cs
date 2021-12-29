using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager pm;
    
    [HideInInspector] public int totalBricks = 0; 
    [HideInInspector] public bool stopShoot = false;
    [HideInInspector] public bool endingStart = false;
    [HideInInspector] public bool isKicked = false;
    [HideInInspector] public bool rotateCamera = false;
    public GameObject rodPiecePrefab,allRodParent,insideBag;
    public GameObject stackHolder, playerHolder;
    public GameObject stackBrick, shootBrick;
    Vector3 stackLocation = Vector3.zero;
    public Transform shootPoint;

    List<GameObject> stacks = new List<GameObject>();
    public int stackCount = -1, stackAdded = 0;
    public GameObject countingCanvas;

    public bool gameStarted = false;
    public float shootTime = 0.15f;

    GameObject[] c_canvas = new GameObject[100];
    GameObject old_canvas = null;
    TextMeshPro tempText;
    public List<GameObject> allRods = new List<GameObject>();
    int j = 0;
    float slowdownFactor = 0.07f, slowdownLenght = 2f;
    float fValue;

    void Awake()
    {
        if (pm == null)
        {
            pm = this;
        }
        
    }
    
    void Start()
    {
        stackLocation = stackHolder.transform.position;
    }

  

    public void DecreaseStack()
    {
        Destroy(stacks[stacks.Count - 1]);
        totalBricks--;
        stackCount--;
        //stacks[stackCount - 1] = null;
        stacks.RemoveAt(stacks.Count - 1);
    }

    void CountingTagChange()
    {
        if (old_canvas != null)
            Destroy(old_canvas);
        
        c_canvas[1].tag = "Old";
        old_canvas = c_canvas[1];
        c_canvas[1] = null;
        stackAdded = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("GoodRod"))
        {
            Destroy(other.gameObject);
            totalBricks++;
           
            stackCount++;
            stackLocation.y  =1.1f;
            stackAdded++;


            if (stackCount <= 0)
            {
                stackLocation = new Vector3(0, stackLocation.y, -0.3f);
                stacks.Add(Instantiate(rodPiecePrefab, stackLocation, Quaternion.Euler(0, 90, 0), insideBag.transform));
                stacks[stackCount].transform.localPosition = new Vector3(0, stackLocation.y, -0.3f);
                stacks[stackCount].transform.localScale   = new Vector3(10, 10, 60);
               
            }
            else
            {
                
                stackLocation = new Vector3(0, stacks[stackCount - 1].transform.localPosition.y, stacks[stackCount-1].transform.localPosition.z - 0.2f);
                stacks.Add(Instantiate(rodPiecePrefab, stackLocation, Quaternion.Euler(0, 90, 0), insideBag.transform));
                stacks[stackCount].transform.localPosition = new Vector3(0, stacks[stackCount - 1].transform.localPosition.y, stacks[stackCount - 1].transform.localPosition.z - 0.2f);
                if (stackCount % 3 == 0) { stacks[stackCount].transform.localPosition = new Vector3(0, stacks[stackCount - 3].transform.localPosition.y+0.2f, -0.3f); }
                
                stacks[stackCount].transform.localScale = new Vector3(10, 10, 60);
            }
        }
        else if(other.transform.CompareTag("BadRod"))
        {
            if (totalBricks > 0)
            {
                Destroy(other.gameObject);
                DecreaseStack();
                //allRods.RemoveAt(allRods.Count - 1);
            }
           
        }

       
    }

   
    void KickTheStack()
    {
        stackHolder.transform.localPosition = Vector3.Lerp(stackHolder.transform.localPosition, new Vector3(0, 0.55f, 6f), 0.1f);
        gameStarted = false;
        rotateCamera = true;
        Invoke("DoSlowmotion", 0.2f);
        GetComponent<Animator>().SetTrigger("kick");
        Invoke("ThrowStack", 0.4f);

        Invoke("TrueVariable", 0.75f);
    }

    void TrueVariable()
    {
        isKicked = true;
    }

    void ThrowStack()
    {
        GameManager.gm.Invoke("BlinkBonusGround", 3.0f);
        
        if (!GameManager.VIBRATION_OFF)
        {
            Taptic.Success();
        }
        for (int i = 0; i < stacks.Count; i++)
        {
            if (stacks[i] != null)
            {
                stacks[i].transform.SetParent(null);
                stacks[i].GetComponentInChildren<Rigidbody>().isKinematic = false;
                // stacks[i].GetComponentInChildren<Rigidbody>().AddForce(new Vector3(0, 0, 10 * Time.deltaTime));
            }
        }

        for ( int i = stacks.Count - 1; i >= 0; i--)
        {
            if (stacks[i] != null)
            {
                stacks[i].GetComponentInChildren<Rigidbody>().AddForce(new Vector3(0, 0, (50000 + (i * 5000)) * Time.deltaTime));
            }
        }
    }

    void DoSlowmotion()
    {
        // print("slow");
        Time.timeScale = slowdownFactor; // 0.07f float variable
        fValue = Time.fixedDeltaTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Invoke("normalMotion", 0.13f);
        
    } 
    void normalMotion()
    {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = fValue;
        
    }
    void DoFastmotion()
    {
        Time.timeScale = 7;
        fValue = Time.fixedDeltaTime;
        Time.fixedDeltaTime = Time.timeScale * 2.5f;
        Invoke("normalMotion", 2f);
    }
}
