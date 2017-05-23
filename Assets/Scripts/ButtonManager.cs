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
            playbutton.interactable = true;
        }
    }

    public void HideScreen()
    {
        // Enable AI
        if (playername.text == "SONIC")
        {
            player.AddComponent<AgentSonic> ();
        }
        else if (playername.text == "KERMIT")
        {
            player.AddComponent<AgentKermit> ();
        }
        player.SetActive(true);
        screen.SetActive(false);
    }
}
