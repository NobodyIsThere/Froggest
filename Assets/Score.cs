using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        // Edge bonus
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse_pos.z = 0;
            RaycastHit2D hit = Physics2D.CircleCast(mouse_pos, 0.1f, Vector2.right, 1 << LayerMask.NameToLayer("Geometry"));
            if (hit.collider != null)
            {
                
            }
        }
    }
}
