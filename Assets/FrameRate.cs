using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour {

    public int frame_rate = 60;

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = frame_rate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
