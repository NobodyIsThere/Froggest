using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    private float tooleft;
    private float toolow;

	void Start()
    {
        Camera camera = Camera.main;
        Vector3 bound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        tooleft = bound.x - 1;
        toolow = bound.y - 1;
    }

	void Update() {
        CheckPos();
	}

    void CheckPos()
    {
        if (transform.position.x <= tooleft || transform.position.y <= toolow)
        {
            Debug.Log("You Lose");
            Debug.Log(toolow);
            Debug.Log(transform.position.y);
            Debug.Log(tooleft);
            Debug.Log(transform.position.x);
            Debug.Break();
        }

    }
}
