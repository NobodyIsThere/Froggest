using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour {

	public GameObject player;
    private string autofilename;
    public Rigidbody2D body;
    //private Dictionary<double, double> playerSpeed; // timestamp, velocity
    //private double startTime;
    public List<float> velocities = new List<float>();


    void Start () {
        //new filename
        autofilename = System.DateTime.Now.ToString("hh:mm:ss"); 
        Debug.Log(autofilename);
        //body = player.GetComponent<Rigidbody2D>();
		//startTime = Time.time;
		//playerSpeed = new Dictionary<double, double>();
        //monitor velocity
        InvokeRepeating("VelocityCheck", 1.0f, 1.0f);
    }

	void VelocityCheck () {
        // Vector2 velocity = body.velocity;
        //double speed = body.velocity.magnitude;
        //double currentTime = Time.time;
        //playerSpeed.Add(currentTime - startTime, speed);
        velocities.Add(body.velocity.magnitude);
        //foreach (var entry in velocities)
        //    Debug.Log(entry);
	}

	public void printToFile() {
		using (StreamWriter file = new StreamWriter(autofilename))
    foreach (var entry in velocities)
        file.WriteLine(entry); 
	}
}
