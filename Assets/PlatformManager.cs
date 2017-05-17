using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public Transform platformprefab;
    public GameObject[] obj;

    public float MinSpawn;
    public float MaxSpawn;

    void Start()
    {
        SpawnPlatforms();
    }

    void SpawnPlatforms()
    {
        Instantiate(obj[Random.Range(0, obj.GetLength(0))], transform.position, Quaternion.identity);
        Invoke("SpawnPlatforms", Random.Range(MinSpawn, MaxSpawn));
    }

}
