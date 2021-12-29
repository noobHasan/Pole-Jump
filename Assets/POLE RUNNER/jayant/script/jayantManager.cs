using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;
[Serializable]
public struct myMaterials
{
    public Material clr1;
    public Material clr2;
}
public class jayantManager : MonoBehaviour
{
    public delegate void pointerUpEvent();//any argument
    public event pointerUpEvent onPointerUp,onPointerDown;

    public static jayantManager instance;
    public GameObject menuUI, gameUI,emptyObj;

    public bool gameStart = false;
    public bool touchPointer = false;

    public jayantPlayer jp;
    public GameObject ragdoll;

    public Slider levelSlider;
    public GameObject scrorer;
    public myMaterials[] mat;

    public GameObject wonUI, loseUI,tapBtn;

    public GameObject hand;
    public Animation circle;

    public GameObject finishRodHolder;
    public Material[] crowdMat;
    public TextMeshProUGUI from, to;

    public static int totalLevel;
    public GameObject pickupEffHolder,floatingText;

    public AudioSource source;
    public AudioClip CollectClips, fail, success,landingSound;

    #region sounds
    public void playCollect() {
        source.PlayOneShot(CollectClips);
    }
    public void playfail()
    {
        source.PlayOneShot(fail);
    }
    public void playSuccess()
    {
        source.PlayOneShot(success);
    }
    public void landingSoundFn()
    {
        source.PlayOneShot(landingSound);
    }
    #endregion 
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }

       /* if (PlayerPrefs.GetInt("setup") == 0)
        {
            PlayerPrefs.SetInt("setup", 1);
            PlayerPrefs.SetInt("levelNum", 1);//first level can also play again

        }*/
        RenderSettings.fog = true;
        finishRodHolder = GameObject.Find("FINISH ROD HOLDER");
    }
    void Start()
    {
        /*if (PlayerPrefs.GetInt("levelNum") != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("levelNum"));
        }*/
        int num = PlayerPrefs.GetInt("levelNum");
        from.text = num.ToString();
        to.text = (num + 1).ToString();
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        hand.transform.position = Input.mousePosition;
        levelSlider.value = jayantPlayer.instance.gameObject.transform.position.z;
    }
    bool onlyOnce = false;
    public void pointerDown() {

        if (!onlyOnce)
        {
            onlyOnce = true;
            gameStart = true;
            jayantPlayer.instance.enabled = true;
            jayantPlayer.instance.an.SetBool("run", true);
            menuUI.SetActive(false);
            gameUI.SetActive(true);
        }
        circle.Play("handUp");

        touchPointer = true;
        if (!jayantPlayer.instance.finishCrossed) {
            onPointerDown?.Invoke(); 
        }
       
        
    }
    public void pointerUp() {
        circle.Play("handDown"); 


        if (!jayantPlayer.instance.finishCrossed) onPointerUp?.Invoke();
        touchPointer = false;
    }

    public void levelCleared() {
        tapBtn.SetActive(false);
        StartCoroutine(lc());
    }
    IEnumerator lc() {
        yield return new WaitForSeconds(1f);
        wonUI.SetActive(true);
    }
    public void levelFailed()
    {
        tapBtn.SetActive(false);

        StartCoroutine(lf());
    }
    IEnumerator lf()
    {
        yield return new WaitForSeconds(1f);
        loseUI.SetActive(true);
    }
    public void retryClicked() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void nextClicked() {

       
            int num = PlayerPrefs.GetInt("levelNum");
            num += 1;

        if (num==11)
        {
            num = 1;
        }
            PlayerPrefs.SetInt("levelNum", num);

        

        SceneManager.LoadScene(PlayerPrefs.GetInt("levelNum"));
    }

    public void changeFinishRodColor(Material mat) {
        for (int i = 0; i < finishRodHolder.transform.childCount; i++)
        {
            finishRodHolder.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = mat;
        }
    }
}
