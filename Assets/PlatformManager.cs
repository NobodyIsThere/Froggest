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
        lastplat = new Vector3((transform.position.x - MinHorzSpread), transform.position.y, transform.position.z);
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
            float platvertspace = Mathf.Clamp(Random.Range(MinVertSpread, MaxVertSpread), transform.position.y, Screen.height);
            platpos = new Vector3(transform.position.x, platvertspace, 0);
            if (platpos.x > (lastplat.x + MinHorzSpread))
            {
                platpos.x = SpreadPlatforms(platpos.x);
                Instantiate(obj[Random.Range(0, obj.GetLength(0))], platpos, Quaternion.identity);
                lastplat = platpos;
            }
        }
            Invoke("SpawnPlatforms", Random.Range(MinSpawnTime, MaxSpawnTime));

    }


    float SpreadPlatforms(float platposx)
    {
        if ((platposx - lastplat.x) > MaxHorzSpread)
        {
            platposx = lastplat.x + MaxHorzSpread;
        }

        return platposx;
    }

}
