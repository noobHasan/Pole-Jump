using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menuScript : MonoBehaviour
{
    public static int totalNumOfLevel = 10;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("setup") == 0)
        {
            PlayerPrefs.SetInt("setup", 1);
            PlayerPrefs.SetInt("levelNum", 1);//first level can also play again
           
        }
    }
    
    void Start()
    {
      
        SceneManager.LoadScene(PlayerPrefs.GetInt("levelNum"));
    }
    private void Update()
    {
        
    }
}
