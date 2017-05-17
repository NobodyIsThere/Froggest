using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public Transform platformprefab;
    public GameObject[] obj;
    private bool clearspace = true;
    Vector3 platpos;

    public float MinSpawn;
    public float MaxSpawn;

    void Start()
    {
        SpawnPlatforms();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        clearspace = false;
        Debug.Log("no space bro");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        clearspace = true;
        Debug.Log("okay go");
    }

    void SpawnPlatforms()
    {
        platpos = new Vector3(transform.position.x, transform.position.y, 0);
        if (clearspace)
        {
            Instantiate(obj[Random.Range(0, obj.GetLength(0))], platpos, Quaternion.identity);
        }
            Invoke("SpawnPlatforms", Random.Range(MinSpawn, MaxSpawn));

    }

}
