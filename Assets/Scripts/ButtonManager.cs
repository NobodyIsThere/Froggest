using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    public GameObject screen;
    public Button playbutton;
    public InputField playername;
    public GameObject player;

    void Start()
    {
        playbutton.interactable = false;
    }

    public void EnablePlay()
    {
        if (playername.text != "")
        {
            Debug.Log(playername.text);
            playbutton.interactable = true;
        }
    }

    public void HideScreen()
    {
        player.SetActive(true);
        screen.SetActive(false);
    }
}
