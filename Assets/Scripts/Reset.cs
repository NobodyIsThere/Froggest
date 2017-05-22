using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

    public GameObject screen;
    public GameObject player;
    
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Debug.Log("You clicked me!!!");
    }

    public void HideScreen()
    {
        player.SetActive(true);
        screen.SetActive(false);
    }
}
