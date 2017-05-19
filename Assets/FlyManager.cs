using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyManager : MonoBehaviour {

    public GameObject obj;
    private bool clearspace = true;
    Vector3 flypos;
    Vector3 lastfly;

    public float MinSpawnTime;
    public float MaxSpawnTime;
    public float MinVertSpread;
    public float MaxVertSpread;

    void Start()
    {
        SpawnFlys();
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

    void SpawnFlys()
    {
        if (clearspace)
        {
            float flyvertspace = Mathf.Clamp(Random.Range(MinVertSpread, MaxVertSpread), 0, Screen.height);
            flypos = new Vector3(transform.position.x, flyvertspace, 0);
            Instantiate(obj, flypos, Quaternion.identity);
        }
        Invoke("SpawnFlys", Random.Range(MinSpawnTime, MaxSpawnTime));
    }


}
