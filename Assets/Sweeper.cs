using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            Debug.Break();
            return;
        }

        if (coll.tag == "Walls")
        {
            return;
        }

        if (coll.gameObject.transform.parent)
        {
            Destroy(coll.gameObject.transform.parent.gameObject);
            Debug.Log("touched it!!");
        }
        else
        {
        Destroy(coll.gameObject);
        Debug.Log("omg touched it!!");
        }
        
    }

}
