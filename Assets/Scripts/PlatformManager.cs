using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public GameObject[] obj;
    private bool clearspace = true;
    Vector3 platpos;
    Vector3 lastplat;

    public float MinSpawnTime;
    public float MaxSpawnTime;
    public float MinVertSpread;
    public float MaxVertSpread;
    public float MinHorzSpread;
    public float MaxHorzSpread;

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
            //Choose a Y value based on the min/max spread
            float platvertspace = Random.Range((lastplat.y - MaxVertSpread), (lastplat.y + MaxVertSpread));

            if (platvertspace > (lastplat.y - MinVertSpread) && platvertspace < (lastplat.y + MinVertSpread))
            {
                if (Random.value < 0.5f)
                    platvertspace = Mathf.Clamp(lastplat.y + MinVertSpread, lastplat.y, topbound.y);
                else
                    platvertspace = Mathf.Clamp(lastplat.y - MinVertSpread, lastplat.y, lowbound.y);
            }
            platvertspace = Mathf.Clamp(platvertspace, lowbound.y, topbound.y);

            platpos = new Vector3(transform.position.x, platvertspace, 0);
            if (platpos.x >= (lastplat.x + MinHorzSpread))
            {
                platpos.x = SpreadPlatforms(platpos.x);
                Instantiate(obj[Random.Range(0, obj.GetLength(0))], platpos, Quaternion.identity);
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
