using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform frog;
    Vector3 target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void Update () {

        if (frog.position.x > transform.position.x)
        {
            target = new Vector3(frog.position.x + 1, 0, -10);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        }

    }

}
