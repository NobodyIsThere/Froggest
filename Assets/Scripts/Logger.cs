using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour {

	public GameObject player;
    public string subjectNumber;
    private string autofilename;
    public Rigidbody2D body;
    //private Dictionary<double, double> playerSpeed; // timestamp, velocity
    //private double startTime;
    public List<float> velocities = new List<float>();
    private int finalDistance;
    private int finalScore;
    private int maxMultiplier;
    private int flyBonus;
    private int edgeBonus;
    private List<Vector2> mousePositions = new List<Vector2>();


    void Start () {
        //new filename
        autofilename = subjectNumber + "_" + System.DateTime.Now.ToString("hh:mm:ss") + ".csv"; 
        Debug.Log(autofilename);
        //body = player.GetComponent<Rigidbody2D>();
		//startTime = Time.time;
		//playerSpeed = new Dictionary<double, double>();
        //monitor velocity
        InvokeRepeating("VelocityCheck", 1.0f, 1.0f);
    }

    void Update () {
        // check if mouse is down, if so register mouseposition
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

    void mousePositionsCheck() {
        // do stuff
    }

	public void printToFile() {
		using (StreamWriter file = new StreamWriter(autofilename)) {
            file.Write("velocities,");
            foreach (var entry in velocities) {
                file.Write(entry);
                file.Write(",");
            }
            file.WriteLine();
            file.WriteLine("finalDistance," + finalDistance);
            file.WriteLine("finalScore," + finalScore);
            file.WriteLine("maxMultiplier," + maxMultiplier);
            file.WriteLine("flyBonus," + flyBonus);
            file.WriteLine("edgeBonus," + edgeBonus);
        }
    } 
}