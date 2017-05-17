using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform frog;
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(frog.position.x + 1, 0, -10);
	}
}
