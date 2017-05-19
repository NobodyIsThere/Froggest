using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentKermit : MonoBehaviour {

	public GameObject camera;
	public GameObject player;
	private Camera cam;
	private float[] midScreenBounds = new float[2]; //left, right
	private GameObject[] platforms;
	private GameObject nextPlatform;

	// Use this for initialization
	void Start () {
		cam = camera.GetComponent<Camera>();
		int screenWidth = cam.pixelWidth;
		int screenHeight = cam.pixelHeight;
		midScreenBounds[0] = screenWidth / 3; // left bound
		midScreenBounds[1] = (screenWidth / 3) * 2; // right bound
		Debug.Log(midScreenBounds[0]);
		Debug.Log(midScreenBounds[1]);
		// if left and right are < 0 then platform is in left part of screen
		// if left > 0 and right < 0 then platform is in middle of screen
		// if left and right are > 0 then platform is in right part of screen
		// currentPlatform = GameObject.
	}
	
	// Update is called once per frame
	void Update () {
			platforms = GameObject.FindGameObjectsWithTag("platform");
			foreach (GameObject platform in platforms) {
				Vector2 position = platform.GetComponent<Transform>().position;
				position = cam.WorldToScreenPoint(position);
				// Debug.Log("position of platform found: " + position);
				float left = position.x - midScreenBounds[0];
				// Debug.Log("position of platform minus left bound: " + left);
				float right = position.x - midScreenBounds[1];
				// Debug.Log("position of platform minus right bound: " + right);
				if (left > 0 && right < 0) {
					Debug.Log("LICKTHEBASTARD!");
					player.GetComponent<PlayerMovement>().Click(position);
				}
		}
	}
}
