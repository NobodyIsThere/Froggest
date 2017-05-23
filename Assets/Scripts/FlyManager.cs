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
    private float radius;


    void Start()
    {
        CircleCollider2D coll = obj.GetComponent<CircleCollider2D>();
        radius = coll.radius + 3;
        SpawnFlys();
    }

    void SpawnFlys()
    {
        //set spawn bounds to min & max y values of screen
        Camera camera = Camera.main;
        Vector3 topbound = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 lowbound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        //choose transform position for new fly, check if space is clear
        float flyvertspace = Random.Range((lowbound.y + radius), (topbound.y - radius));
        flypos = new Vector3(transform.position.x, flyvertspace, 0);
        clearspace = Clear(flypos);

        if (clearspace)
        {
            //check distance from last fly
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
        var hitColliders = Physics.OverlapSphere(space, radius);
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
