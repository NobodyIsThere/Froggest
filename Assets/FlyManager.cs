using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyManager : MonoBehaviour {

    public GameObject obj;
    private bool clearspace = true;
    Vector3 flypos;
    Vector3 lastfly;
    Vector3 topbound;
    Vector3 lowbound;

    public float MinSpawnTime;
    public float MaxSpawnTime;
    public float MinHorzSpread;


    void Start()
    {
        SpawnFlys();
    }

    void SpawnFlys()
    {
        Camera camera = Camera.main;
        Vector3 topbound = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 lowbound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        float flyvertspace = Random.Range(lowbound.y, topbound.y);
        //Debug.Log(flyvertspace);
        flypos = new Vector3(transform.position.x, flyvertspace, 0);
        clearspace = Clear(flypos);

        if (clearspace)
        {
            if (flypos.x > (lastfly.x + MinHorzSpread))
            {
                Instantiate(obj, flypos, Quaternion.identity);
                lastfly = flypos;
            }
        }
        Invoke("SpawnFlys", Random.Range(MinSpawnTime, MaxSpawnTime));
    }

    bool Clear(Vector3 space)
    {
        bool spaceclear;
        var hitColliders = Physics.OverlapSphere(space, 3);//2 is purely chosen arbitrarly
        if (hitColliders.Length > 0)
        {
            spaceclear = false;
                
        }

        else
        {
            spaceclear = true;
        }
            return spaceclear;
    }

}
