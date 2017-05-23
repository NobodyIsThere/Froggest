using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public GameObject[] obj;
    public bool IsRaining = false;
    public bool IsSnowing = false;
    public GameObject SplashSystem;

    private bool clearspace = true;
    Vector3 platpos;
    Vector3 lastplat;

    public float MinSpawnTime;
    public float MaxSpawnTime;
    public float MinVertSpread;
    public float MaxVertSpread;
    public float MinHorzSpread;
    public float MaxHorzSpread;
    public float BufferSpace; // keep platforms spawning too close to top/bottom screen edges

    void Start()
    {
        Camera camera = Camera.main;
        Vector3 topbound = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 lowbound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        lastplat = new Vector3((transform.position.x - MinHorzSpread), transform.position.y, transform.position.z);
        SpawnPlatforms();
    }

    //check if trying to spawn within a preexisting platform
    void OnTriggerEnter2D(Collider2D other)
    {
        clearspace = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        clearspace = true;
    }

    //attempt to spawn a platform if space seems clear
    void SpawnPlatforms()
    {
        Camera camera = Camera.main;
        Vector3 topbound = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 lowbound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        //check space is clear
        if (clearspace)
        {
            //Choose a Y value between the maximum possible value below & above the last platform
            float platvertspace = Random.Range((lastplat.y - MaxVertSpread), (lastplat.y + MaxVertSpread));

            //adjust vert space if minimum vertical spacing not met
            if (platvertspace > (lastplat.y - MinVertSpread) && platvertspace < (lastplat.y + MinVertSpread))
            {
                if (Random.value < 0.5f)
                    platvertspace = Mathf.Clamp(lastplat.y + MinVertSpread, lastplat.y, (topbound.y - BufferSpace));
                else
                    platvertspace = Mathf.Clamp(lastplat.y - MinVertSpread, lastplat.y, (lowbound.y + BufferSpace));
                //Debug.Log(topbound.y);
            }
            //clamp platform Y to screen bounds
            platvertspace = Mathf.Clamp(platvertspace, (lowbound.y + BufferSpace), (topbound.y - BufferSpace));

            platpos = new Vector3(transform.position.x, platvertspace, 0);
            if (platpos.x >= (lastplat.x + MinHorzSpread))
            {
                platpos.x = SpreadPlatforms(platpos.x);
                GameObject platform = Instantiate(obj[Random.Range(0, obj.Length)], platpos, Quaternion.identity);
                if (IsRaining)
                {
                    int num_blocks = platform.transform.childCount;
                    GameObject splash = Instantiate(SplashSystem, platform.transform);
                    splash.transform.localPosition = new Vector3(0f, 0.6f, 0.5f);
                    ParticleSystem.ShapeModule shape = splash.GetComponent<ParticleSystem>().shape;
                    shape.radius = platform.GetComponent<BoxCollider2D>().size.x*0.5f;
                    splash.GetComponent<ParticleSystem>().Play();
                }
                lastplat = platpos;
            }
        }
            Invoke("SpawnPlatforms", Random.Range(MinSpawnTime, MaxSpawnTime));

    }

    //check horizontal distance between last platform and current platform, adjust if too large
    float SpreadPlatforms(float platposx)
    {
        if ((platposx - lastplat.x) > MaxHorzSpread)
        {
            platposx = lastplat.x + MaxHorzSpread;
        }
        return platposx;
    }

}
