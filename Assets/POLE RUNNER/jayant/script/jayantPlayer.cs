using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jayantPlayer : MonoBehaviour
{
    public static jayantPlayer instance;
    #region leftRightVar
    float h, width;
    bool onlyOnce = false;
    public float speed = 4f;
    Vector3 pos;
    Vector3 desiredPos;

    #endregion
    public Animator an;
    CharacterController cc;
    Material myMat;
    public GameObject stackPointer, poleHolder;
    public int stackCounter = 0, poleCounter = 0;
    bool canCreateRod = false, fly = false;
    float poleAddRate = 0.1f;
    Vector3 rotArndPoint;
    public GameObject polePeice;
   public GameObject pole;//seperate
    public GameObject poleTopObj;
    public Vector3 offsetWithPole;
    public Vector3 rotOffsetWithPole;
    public bool stopRot=false;
    bool applyForce=false;
    Vector3 playerVelocity;
    public GameObject smoke,stackHolder;
    public bool finishCrossed = false;
    public float gravity = -20;
    bool shouldMoveLeftRight=true;
    public GameObject confiti;
    Vector3 impactDirVAl;
    Vector3 forward;
    bool lastfly = false;


    private void Awake()
    {
        instance = this;
        an.gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        forward = new Vector3(0f,-0.1f,1f);
    }
    void Start()
    {
        jayantManager.instance.onPointerUp += pointerUpEvent;
        jayantManager.instance.onPointerDown += pointerDownEvent;
        width = Screen.width;
        cc = GetComponent<CharacterController>();
        myMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        jayantManager.instance.changeFinishRodColor(myMat);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!fly && !applyForce)
        {

            cc.Move(forward * Time.deltaTime * speed);
            smoke.SetActive(true);
        }
        else {
            smoke.SetActive(false);
        }

        if (finishCrossed)
            shouldMoveLeftRight = true;
        else
            shouldMoveLeftRight = (!fly /*&& !applyForce*/);

        if (jayantManager.instance.touchPointer&& shouldMoveLeftRight)
        {

            print("left right");
           

                #region leftRightMovementCode
                if (!onlyOnce)
                {
                    onlyOnce = true;
                    h = Input.mousePosition.x;
                    pos = transform.position;
                   
                }
                float p = (Input.mousePosition.x - h) / width;

         
            playerVelocity.x = p * 40;
            p *= 25;
                desiredPos = new Vector3(p + pos.x, cc.transform.position.y, cc.transform.position.z);
                desiredPos.x = Mathf.Clamp(desiredPos.x, -4.2f, 4.2f);
                //cc.transform.position = desiredPos;
              if(!lastfly) cc.Move(desiredPos-transform.position);
                #endregion
           

        }


        if (canCreateRod)
        {
            print("creating rod");
            poleAddRate -= Time.deltaTime;
            if (poleAddRate < 0)
            {
                poleAddRate = 0.1f;
                addAPoleElement();
            }
        }

        if (fly&& poleTopObj!=null)//time to fly
        {
            print("fly");
            Vector3 pos = poleTopObj.transform.position + offsetWithPole;
          
                transform.position =  pos;
           
            transform.eulerAngles = rotOffsetWithPole;
           

            if (!stopRot)
            {
                if (!finishCrossed)
                {
                    pole.transform.RotateAround(rotArndPoint , Vector3.right, 3*Time.deltaTime*20);

                }
                else {
                    float x = pole.transform.localEulerAngles.x;

                    if (x>89&&x<360)
                    {
                        lastfly = true;
                        print("last fly" + x);
                        an.SetBool("fall", true);
                        stopRot = true;
                    }
                    if (x < 90)
                    {
                        pole.transform.RotateAround(rotArndPoint , Vector3.right, 3 * Time.deltaTime *20);
                    }
                   /* else {
                        lastfly = true;
                        print("last fly"+x);
                        an.SetBool("fall", true);
                        stopRot = true;
                        
                    }*/
                }
               
            }
            else {
                impactDirVAl = poleTopObj.transform.up;
                print("POLE TOP object dir"+impactDirVAl);
                poleTopObj = null;
                fly = false;
                playerVelocity.y = 0;
                applyForce = true;
            }
        }

        if (applyForce)
        {
            Vector3 impactDir = impactDirVAl;
            if (!finishCrossed)
            {
                impactDir *= 2f;
            }
            else
            {
                impactDir *= 3f;
                gravity = -1;
            }
            cc.Move(impactDir * Time.deltaTime * speed);

            playerVelocity.y += gravity * Time.deltaTime;
            cc.Move(playerVelocity * Time.deltaTime);
            if (!finishCrossed)
            {
                Vector3 pos = cc.transform.position;
                pos.x = Mathf.Clamp(pos.x, -4.2f, 4.2f);
                cc.transform.position = pos;
                //cc.Move(pos-cc.transform.position);
            }

        }
       
    }
    bool workFine = true;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("pickup"))
        {
           
            if (other.GetComponent<MeshRenderer>().material.color == myMat.color)
            {
                Taptic.Light();
                GameObject eff = Instantiate(jayantManager.instance.pickupEffHolder, other.transform.position, other.transform.rotation);
                //eff.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = other.gameObject.GetComponent<MeshRenderer>().material.color;
                Destroy(eff,2f);
                addStackHolder(other.gameObject);
            }
            else
            {
                Taptic.Failure();

                deleteFromStacks();
            }
        }

        if (other.CompareTag("ground"))
        {
            jayantManager.instance.landingSoundFn();
            h = Input.mousePosition.x;
            pos = transform.position;
            stopRot = false;
            applyForce = false;
            transform.eulerAngles = Vector3.zero;
            transform.position = new Vector3(transform.position.x,0f,transform.position.z);

            fly = false;
            an.SetBool("fly", false);
        }

        if (other.CompareTag("Finish"))
        {
            finishCrossed = true;
            speed *= 2f;
           
            Invoke("pointerUpEvent",3f);
        }

        if (other.CompareTag("hurdle"))
        {
           
            print("player:hurdle");
            RotateBlade[] rb = FindObjectsOfType<RotateBlade>();
            foreach (var r in rb)
            {
                r.speed = 0;
            }

            other.isTrigger = false;
            died();
        }

        if (other.CompareTag("water"))
        {
            jayantManager.instance.playSuccess();
            confiti.SetActive(true);
            an.Rebind();
            an.SetBool("dance",true);
            transform.eulerAngles = new Vector3(0,180,0);
            transform.position = transform.position + Vector3.up;
            jayantManager.instance.levelCleared();
            this.enabled = false;
        }

        if (other.CompareTag("clrChange"))
        {
            myMat = other.gameObject.GetComponentInParent<clrChangeScript>().changeMat;
            GetComponentInChildren<SkinnedMeshRenderer>().material = myMat;
            jayantManager.instance.changeFinishRodColor(myMat);

            for (int i = 0; i < stackPointer.transform.childCount; i++)
            {
                stackPointer.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = myMat;
            }
            for (int i = 0; i < poleHolder.transform.childCount; i++)
            {
                poleHolder.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = myMat;
            }
        }
    }

    public void addStackHolder(GameObject g)
    {
        jayantManager.instance.playCollect();
        g.tag = "Untagged";
        g.transform.position = stackPointer.transform.position;
        g.transform.SetParent(stackPointer.transform);
        g.transform.localScale = new Vector3(0.4786102f, 0.47861f, 2.14f);// 0.2481593f, 0.2481592f, 0.9926372f);
        g.transform.localEulerAngles = Vector3.zero;

        Vector3 localPos = g.transform.localPosition;
        localPos.y = stackCounter * .009f;// 0.00492f;
        g.transform.localPosition = localPos;
        Destroy(Instantiate(jayantManager.instance.floatingText,transform.position+Vector3.up*5,transform.rotation),1f);
        stackCounter += 1;
    }
    public void deleteFromStacks()
    {
        print("delete stack");
        int childCount = stackPointer.transform.childCount;
        print(childCount);
        if (childCount > 0)
        {
            Destroy(stackPointer.transform.GetChild(childCount - 1).gameObject);
            stackCounter -= 1;
        }
        else
        {
            //die
            died();
        }

    }


    public void pointerUpEvent()
    {
        onlyOnce = false;
        if (stackCounter > 2&&!fly && !applyForce)
        {
            canCreateRod = true;
            an.SetBool("run", false); an.SetBool("poleRun", true);
            speed -= 1;
        }

        if (finishCrossed)
        {
            an.SetBool("run", false); an.SetBool("poleRun", true);
        }

    }
    public void pointerDownEvent()
    {
        print("PointerDownPlayer");
        if (canCreateRod )
        {
            fly = true;
            rotArndPoint = transform.position;
            rotArndPoint.y = 1;
            rotArndPoint.z += poleCounter * 1.2455f;
            if (pole != null) Destroy(pole);
            pole = Instantiate(jayantManager.instance.emptyObj, poleHolder.transform.position, poleHolder.transform.rotation);
            int l = poleHolder.transform.childCount;

            for (int i = 0; i < l; i++)
            {
                poleHolder.transform.GetChild(0).transform.SetParent(pole.transform);
               // GameObject pp = Instantiate(polePeice, poleHolder.transform.GetChild(0).transform.position, poleHolder.transform.GetChild(0).transform.rotation);
               // pp.GetComponent<copyPolePartPos>().target = poleHolder.transform.GetChild(0).gameObject;
               // pp.GetComponent<copyPolePartPos>().lerpValue = i + 1;
            }
            poleTopObj = pole.transform.GetChild(0).gameObject;

            an.SetBool("fly", true);
            an.SetBool("poleRun", false);

        }
        else
        {
            an.SetBool("run", true); an.SetBool("poleRun", false);
        }

        canCreateRod = false;
        poleCounter = 0;
        speed = 10;

    }
    public void addAPoleElement()
    {
        if (stackCounter > 0)
        {
            Taptic.Light();

            int childCount = stackPointer.transform.childCount;
            GameObject topObj = stackPointer.transform.GetChild(childCount - 1).gameObject;//top object of stack

            topObj.transform.SetParent(poleHolder.transform);
            topObj.transform.position = poleHolder.transform.position;
            topObj.transform.localScale = new Vector3(31.25407f, 31.25407f, 125.0163f);
            topObj.transform.localEulerAngles = Vector3.zero;

            Vector3 localPos = topObj.transform.localPosition;
            localPos.z = poleCounter * 2.491f;
            topObj.transform.localPosition = localPos;


            poleCounter += 1;
            stackCounter -= 1;
        }
    }

    public void destroyPole() {
        if (pole != null)
        {
            if (pole.transform.childCount > 0)
            {
                for (int i = 0; i < pole.transform.childCount; i++)
                {
                    pole.transform.GetChild(i).gameObject.GetComponent<collisionDetector>().canCheckHurdle = false;
                    pole.transform.GetChild(i).gameObject.GetComponent<Collider>().isTrigger = false;
                    if (pole.transform.GetChild(i).gameObject.GetComponent<Rigidbody>() == null)
                    {
                        pole.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                    }

                    //  pole.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward*3,ForceMode.Impulse) ;
                    // pole.transform.GetChild(i).gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right*3, ForceMode.Impulse);
                }
            }
        }
      
    }

    void died() {
        jayantManager.instance.playfail();
        Taptic.Failure();

        stackHolder.transform.SetParent(null);
        spawnRagdool(transform.position, transform.rotation);
        

        stackHolder.GetComponentInChildren<Rigidbody>().isKinematic = false;
        print(stackPointer.transform.childCount);
        for (int i = 0; i < stackPointer.transform.childCount; i++)
        {
           
            stackPointer.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            stackPointer.transform.GetChild(i).gameObject.GetComponent<Collider>().isTrigger = false;
           //  stackPointer.transform.GetChild(0).transform.SetParent(null);
        }

        gameObject.SetActive(false);

        jayantManager.instance.levelFailed();
    }

    public void spawnRagdool(Vector3 pos, Quaternion rot)
    {
        GameObject rd= Instantiate(jayantManager.instance.ragdoll, pos, rot);
        rd.GetComponentInChildren<SkinnedMeshRenderer>().material = myMat;
        Vector3 explosionPos = pos;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null) {
                 rb.AddExplosionForce(1050, explosionPos+Vector3.back, 20, 1.0f);
                
            }
        }
    }
}
