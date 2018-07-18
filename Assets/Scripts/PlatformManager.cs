using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public GameObject[] obj;
    public WeatherScript.Weather Weather;
    public GameObject SplashSystem;
    public GameObject Snow;

    private bool clearspace = true;
    private new Camera camera;
    Vector3 platpos;
    Vector3 lastplat;

    public float next_pos;

    public float MinSpawnDist = 5;
    public float MaxSpawnDist = 10;
    public float MinVertSpread;
    public float MaxVertSpread;
    public float MinHorzSpread;
    public float MaxHorzSpread;
    public float BufferSpace; // keep platforms spawning too close to top/bottom screen edges

    void Start()
    {
        camera = Camera.main;
        lastplat = new Vector3((transform.position.x - MinHorzSpread), transform.position.y, 0);
        //SpawnPlatforms();
        next_pos = GetNextPos();
    }

    void Update()
    {
        if (transform.parent.position.x > next_pos)
        {
            SpawnPlatform();
            next_pos = GetNextPos();
        }
    }

    private float GetNextPos()
    {
        // Returns next x-coordinate at which to spawn a platform.
        return transform.parent.position.x + Random.Range(MinSpawnDist, MaxSpawnDist);
    }

    private void SpawnPlatform()
    {
        // Spawn a single platform, based on Thryn's code below (but no need to check for overlap)
        Vector3 topbound = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 lowbound = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        // Pick y-coordinate
        if (Random.value < 0.5f)
            platpos.y = Mathf.Clamp(lastplat.y + Random.Range(MinVertSpread, MaxVertSpread), lastplat.y, topbound.y - BufferSpace);
        else
            platpos.y = Mathf.Clamp(lastplat.y - Random.Range(MinVertSpread, MaxVertSpread), lowbound.y + BufferSpace, lastplat.y);

        platpos.x = transform.position.x;

        GameObject platform = Instantiate(obj[Random.Range(0, obj.Length)], platpos, Quaternion.identity);
        switch (Weather)
        {
            case WeatherScript.Weather.RAIN:
                int num_blocks = platform.transform.childCount;
                GameObject splash = Instantiate(SplashSystem, platform.transform);
                splash.transform.localPosition = new Vector3(0f, 0.6f, 0.5f);
                ParticleSystem.ShapeModule shape = splash.GetComponent<ParticleSystem>().shape;
                shape.radius = platform.GetComponent<BoxCollider2D>().size.x * 0.5f;
                splash.GetComponent<ParticleSystem>().Play();
                break;
            case WeatherScript.Weather.SNOW:
                foreach (Transform t in platform.transform)
                {
                    Instantiate(Snow, t);
                }
                break;
            default:
                break;
        }
        lastplat = platpos;
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
        camera = Camera.main;
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
                switch (Weather)
                {
                    case WeatherScript.Weather.RAIN:
                        int num_blocks = platform.transform.childCount;
                        GameObject splash = Instantiate(SplashSystem, platform.transform);
                        splash.transform.localPosition = new Vector3(0f, 0.6f, 0.5f);
                        ParticleSystem.ShapeModule shape = splash.GetComponent<ParticleSystem>().shape;
                        shape.radius = platform.GetComponent<BoxCollider2D>().size.x*0.5f;
                        splash.GetComponent<ParticleSystem>().Play();
                        break;
                    case WeatherScript.Weather.SNOW:
                        foreach (Transform t in platform.transform)
                        {
                            Instantiate(Snow, t);
                        }
                        break;
                    default:
                        break;
                }
                lastplat = platpos;
            }
        }
        // Invoke("SpawnPlatforms", Random.Range(MinSpawnTime, MaxSpawnTime));

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
