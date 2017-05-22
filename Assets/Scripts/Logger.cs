using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger : MonoBehaviour {

	public GameObject player;
    public string subjectNumber;
    private string autofilename;
    public Rigidbody2D body;
    public Transform playerTransform;
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
<<<<<<< HEAD
=======
        PlayerMovement controller = playerTransform.GetComponent<PlayerMovement>();
>>>>>>> 5e7c872e00f5a1676c3133331c32fca9a9e586dc
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

<<<<<<< HEAD
	public void printToFile() {
=======
	public void printToFile(int _finalDistance, int _finalScore, int _maxMultiplier, int _numFlies, int _numEdges) {
>>>>>>> 5e7c872e00f5a1676c3133331c32fca9a9e586dc
		using (StreamWriter file = new StreamWriter(autofilename)) {
            file.Write("velocities,");
            foreach (var entry in velocities) {
                file.Write(entry);
<<<<<<< HEAD
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
=======
                // add comma unless it's the last entry in the list
                if (velocities.IndexOf(entry) != velocities.Count - 1) {
                    file.Write(",");
                }
            }
            file.WriteLine();
            file.WriteLine("finalDistance," + _finalDistance);
            file.WriteLine("finalScore," + _finalScore);
            file.WriteLine("maxMultiplier," + _maxMultiplier);
            file.WriteLine("flyBonus," + _numFlies);
            file.WriteLine("edgeBonus," + _numEdges);
        }
    }

    public void setSubjectNumber(string subject) {
        subjectNumber = subject;
    }
>>>>>>> 5e7c872e00f5a1676c3133331c32fca9a9e586dc
}