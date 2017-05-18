using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public Transform platformprefab;
    public GameObject[] obj;
    private bool clearspace = true;
    private Camera cam = Camera.main;
    Vector3 platpos;

    public float MinSpawnTime;
    public float MaxSpawnTime;
    public float MinVertSpread;
    public float MaxVertSpread;

    void Start()
    {
        SpawnPlatforms();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        clearspace = false;
        //Debug.Log("no space bro");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        clearspace = true;
        //Debug.Log("okay go");
    }

    void SpawnPlatforms()
    {
        if (clearspace)
        {
            float platspace = Mathf.Clamp(Random.Range(MinVertSpread, MaxVertSpread), transform.position.y, Screen.height);
            platpos = new Vector3(transform.position.x, platspace, 0);
            Instantiate(obj[Random.Range(0, obj.GetLength(0))], platpos, Quaternion.identity);
        }
            Invoke("SpawnPlatforms", Random.Range(MinSpawnTime, MaxSpawnTime));

    }

}
