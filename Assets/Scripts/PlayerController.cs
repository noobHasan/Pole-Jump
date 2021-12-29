using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController pc;
    bool afterClicked = false;
    float h, width;
    bool onlyOnce = false, jumping = false, canFly = false, rolled = false, creatingRod = false,added = false,rodCreated = false;
    public GameObject playerHolder, mainPlayer;
    public float speed = 8f;
   // float Animation;
    public GameObject cube; 
    Vector3 pos;
    Vector3  desiredPos;
    public GameObject rightArm;
    public Animator anim;
    public CharacterController cc;
    Vector3 playerPos;

    public bool gameStarted;
    private void Awake() {
        if (pc == null)
        {
            pc = this;
        }
    }
    
    void Start()
    {
        width = Screen.width;
        anim = mainPlayer.GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    bool myONce = false;
    void Update()
    {
      
        if (gameStarted)
        {
            if (cc.isGrounded)
            {
                if (canFly&&!myONce)
                {
                    myONce = true;
                    anim.SetBool("jump", false);
                    StartCoroutine(AfterDelayRoll(PlayerManager.pm.allRods.Count / 10));
                    transform.GetChild(0).transform.localPosition = Vector3.Lerp(transform.GetChild(0).transform.localPosition, new Vector3(transform.GetChild(0).transform.localPosition.x, -1, transform.GetChild(0).transform.localPosition.z), 1f * Time.deltaTime);
                    afterClicked = false;

                    for (int i = 0; i < PlayerManager.pm.allRods.Count; i++)
                    {
                        PlayerManager.pm.allRods[i].AddComponent<Rigidbody>();
                    }
                    PlayerManager.pm.allRodParent.transform.parent = null;
                }
            }
            else
            {
               
            }



            if (!creatingRod)
            anim.SetBool("run", true);


            if (!canFly)
            {
                playerPos = transform.position;
            }
            if (Input.GetMouseButtonDown(0))
            {
                afterClicked = true;
                anim.SetBool("PoleInHand", false);
                h = Input.mousePosition.x;
                pos = transform.position;               
            }
            if (Input.GetMouseButton(0))
            {
                StopAllCoroutines();
                if (creatingRod)
                {
                    canFly = true;
                    onlyOnce = false;
                }
                float p = (h - Input.mousePosition.x) / width;
                p *= -15;
                desiredPos = pos + new Vector3(p,0, 0);
                desiredPos.x = Mathf.Clamp(desiredPos.x, -4.2f, 4.2f);
                // playerHolder.transform.localPosition = new Vector3(desiredPos.x, desiredPos.y, playerHolder.transform.localPosition.z);
                // cc.Move(desiredPos - transform.position);
                
            }
            else
            {
                if (afterClicked)
                {
                    if (!onlyOnce && !canFly) {
                        creatingRod = true;
                        onlyOnce = true;
                        anim.SetBool("run", false);
                        anim.SetBool("PoleInHand", true);
                        StartCoroutine(afterDelayAddRod());
                    }
                }
            }
            if(canFly)
            {
                creatingRod = false;
                rodCreated = false;
                anim.SetBool("run", false);
                PlayerManager.pm.allRodParent.transform.localRotation = Quaternion.Lerp(PlayerManager.pm.allRodParent.transform.localRotation, Quaternion.Euler(90, 0, 0), 1 * Time.deltaTime);
                jumping = true;
               // Animation += Time.deltaTime;
               // Animation = Animation % 2f;
                transform.position = MathParabola.Parabola(playerPos, playerPos + Vector3.forward * (PlayerManager.pm.allRods.Count * 2.5f), PlayerManager.pm.allRods.Count / 1.5f, .5f );
               
               
                if (!onlyOnce)
                {
                    onlyOnce = true;
                    anim.SetBool("jump", true);
                    anim.SetBool("PoleInHand", false);
                }
            }
            if (!jumping || rolled)
            {
                Debug.Log("Moving");
                cc.Move(new Vector3(desiredPos.x - transform.position.x, desiredPos.y - transform.position.y - 2, speed * Time.deltaTime));
            }
        }
        else
        {
            anim.SetBool("run", false);
        }

    }
    IEnumerator afterDelayAddRod()
    {
        int num = PlayerManager.pm.stackCount;
        while (PlayerManager.pm.stackCount >= 0)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject rod = Instantiate(PlayerManager.pm.rodPiecePrefab, transform, PlayerManager.pm.allRodParent.transform);
            rod.transform.parent = PlayerManager.pm.allRodParent.transform;
            //rod.transform.localPosition = new Vector3(0.165f, 1.165f, (PlayerManager.pm.stackCount-PlayerManager.pm.stackCount-1) * 1.2f);
            rod.transform.localPosition = new Vector3(0, -0.0002f, (num-PlayerManager.pm.stackCount+1) * 1.2f);
            rod.transform.localScale = new Vector3(7.4f, 7.4f, 74);
            rod.transform.localRotation = Quaternion.Euler(-180, 0, 0);
            PlayerManager.pm.allRods.Add(rod);
            PlayerManager.pm.DecreaseStack();
            //
        }
        //rodCreated = true;
        
    }
    IEnumerator AfterDelayRoll(float time)
    {
       // yield return new WaitForSeconds(time);
        //anim.SetBool("rolling", true);
        anim.SetBool("jump", false);
        rolled = true;
        yield return new WaitForSeconds(time+1f);
        anim.SetBool("rolling", false);
        jumping = false;
        canFly = false;
        creatingRod = false;
        PlayerManager.pm.totalBricks = 0;
        PlayerManager.pm.allRodParent.transform.position = new Vector3(0, 0, 0);
    }

    public bool pointerDown = false;
    bool firstTime=false;

    public GameObject menuUI, gameUI;
    public void pointerDownFn() {
        if (!firstTime)
        {
            firstTime = true;
            gameStarted = true;
            menuUI.SetActive(false);
            gameUI.SetActive(true);
        }
        pointerDown = true;
    }
    public void pointerUpFn()
    {
        pointerDown = false;
    }
}
