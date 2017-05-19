using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour {

	public GameObject player;
	Rigidbody2D body;
	private Dictionary<double, double> playerSpeed; // timestamp, velocity
	private double startTime;

	// Use this for initialization
	void Start () {
		body = player.GetComponent<Rigidbody2D>();
		startTime = Time.time;
		playerSpeed = new Dictionary<double, double>();
	}
	
	// Update is called once per frame
	void Update () {
		// Vector2 velocity = body.velocity;
		double speed = body.velocity.magnitude;
		double currentTime = Time.time;
		playerSpeed.Add(currentTime - startTime, speed);
	}

	public void printToFile() {
		using (StreamWriter file = new StreamWriter("logtest.txt"))
    foreach (var entry in playerSpeed)
        file.WriteLine("[{0} {1}]", entry.Key, entry.Value); 
	}
}
