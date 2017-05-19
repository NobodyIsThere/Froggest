﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{

    private float tooleft;
    private float toolow;

<<<<<<< HEAD
    void Update()
    {
=======
    void Start()
    {
        Camera camera = Camera.main;
        Vector3 bound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        tooleft = bound.x - 1;
        toolow = bound.y - 1;
    }

    void Update() {
>>>>>>> 6e54fb38d504b8839f7fcaf3983697eeaef2026c
        CheckPos();
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
            Debug.Break();
            //Log Data
        }

    }
}