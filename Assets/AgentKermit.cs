using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentKermit : MonoBehaviour {

	public GameObject camera;
	public GameObject player;
	private Camera cam;
	private PlayerMovement controller;
	private float[] midScreenBounds = new float[2]; //left, right
	private int centerScreenLine;
	private GameObject[] platforms;
	private GameObject nextPlatform;

	// Use this for initialization
	void Start () {
		controller = player.GetComponent<PlayerMovement>();
		cam = camera.GetComponent<Camera>();
		int screenWidth = cam.pixelWidth;
		int screenHeight = cam.pixelHeight;
		centerScreenLine = cam.pixelWidth / 2;
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

		Vector2 playerPosition = player.GetComponent<Transform>().position;
		playerPosition = cam.WorldToScreenPoint(playerPosition);

		if (playerPosition.y > 30) {
			Debug.Log("playerposition: " + playerPosition);
			platforms = GameObject.FindGameObjectsWithTag("platform");
			foreach (GameObject platform in platforms) {

				Vector2 platformPosition = platform.GetComponent<Transform>().position;
				platformPosition = cam.WorldToScreenPoint(platformPosition);
				float left = platformPosition.x - midScreenBounds[0];
				float right = platformPosition.x - midScreenBounds[1];

				if (left > 0 && right < 0) {
					
					Debug.Log("LICKTHEBASTARD!");
					// if (!controller.IsFakeClicking()) {
						controller.Click(platformPosition);
					// } else {

						// if (playerPosition.x > platformPosition.x) {
						 	// controller.Release();
					// }
				}
			}
		}



			// if player is not holding, find middle platform and click it
			// NOPE: if player is holding, release if past middle of platform
			// if player is holding, release if past middle part of screen
			// if speed is low keep finding middle platform
			// if speed is okay, hold for longer
			// if (!controller.IsFakeClicking()) {
			// 	Vector2 platformPosition = nextPlatform.GetComponent<Transform>().position;
			// 	Debug.Log("LICKTHEBASTARD!");
			// 	controller.Click(platformPosition);
			// } else {
			// 	Debug.Log("not fake clicking now");
			// 	// Vector2 playerPosition = player.GetComponent<Transform>().position;
			// 	// playerPosition = cam.WorldToScreenPoint(playerPosition);
			// 	// if (playerPosition.x > centerScreenLine) {
			// 	// 	controller.Release();
			// 	// } else {
			// 	// 	float playerSpeedX = player.GetComponent<Rigidbody2D>().velocity.x;
			// 	// 	Debug.Log("playerspeed in x vector is: " + playerSpeedX);
			// 	// 	if (playerSpeedX < 3) {
			// 	// 		controller.Release();
			// 	// 	}
			// 	// }
			// }

				// float playerSpeed = player.GetComponent<Rigidbody2D>().velocity.magnitude;
				// if (playerSpeed < 0.5) {
				// 	player.GetComponent<PlayerMovement>().Release();
				// }
				// Vector3 bound = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
				// Vector2 lowerBound = cam.WorldToScreenPoint(bound);
        // float tooLow = lowerBound.y - 1;
				// Debug.Log("lowerbound point: " + lowerBound);

		}
}
