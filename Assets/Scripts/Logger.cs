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
    public List<float> velocities = new List<float>();
    private int finalDistance;
    private int finalScore;
    private int maxMultiplier;
    private int flyBonus;
    private int edgeBonus;
    private List<Vector2> mousePositions = new List<Vector2>();
    private List<float> mousePositionTimes = new List<float>();
    private float startTime;


    void Start () {
        //new filename
        autofilename = subjectNumber + "_" + System.DateTime.Now.ToString("hh_mm_ss") + ".csv"; 
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

    public void registerMouseCoordinates(Vector2 position) {
        mousePositions.Add(position);
    }

	public void printToFile(int _finalDistance, int _finalScore, int _maxMultiplier, int _numFlies, int _numEdges) {
		using (StreamWriter file = new StreamWriter(autofilename)) {
            file.Write("velocities,");
            foreach (var entry in velocities) {
                file.Write(entry);
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
            file.Write("mousepos,");
            foreach (var pos in mousePositions) {
                file.Write(pos.ToString());
                // add comma unless it's the last entry in the list
                if (mousePositions.IndexOf(pos) != mousePositions.Count - 1) {
                    file.Write(",");
                }
            }
        }
    }

    public void setSubjectNumber(string subject) {
        subjectNumber = subject;
    }
}