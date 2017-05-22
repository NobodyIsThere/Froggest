using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{

    public Logger logger;
    public GameObject arrow;
    private Transform arrowpos;

    private float tooleft;
    private float toolow;
    public float safezone;
    public GameObject eventSystem;

    void Start()
    {
        //Set camera 
        Camera camera = Camera.main;
        Vector3 bound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        tooleft = bound.x - safezone;
        toolow = bound.y - safezone;
    }

    void Update() {
        CheckPos();
        SetArrow();
    }

    void CheckPos()
    {
        // Find bottom left value of current camera position
        Camera cam = Camera.main;
        Vector3 bound = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        //Set out of bounds to 1 unit off screen 
        tooleft = bound.x - 1;
        toolow = bound.y - 1;

        //if out of bounds, end game & log data
        if (transform.position.x <= tooleft || transform.position.y <= toolow)
        {
            transform.position = new Vector3(tooleft + 1, toolow + 1);
            GetComponent<Score>().EndGame();
            //Log Data
            eventSystem.GetComponent<Logger>().printToFile();
        }

    }

    void SetArrow()
    {
        arrowpos = arrow.transform;
        arrowpos.position = new Vector3(transform.position.x, arrowpos.position.y, 0);
        //if off top screen, activate arrow
        if (OnScreen() && arrow.activeSelf)
            arrow.SetActive(false);
        if (!OnScreen() && !arrow.activeSelf)
            arrow.SetActive(true);
    }

    private bool OnScreen()
    {
        return transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;
    }
}