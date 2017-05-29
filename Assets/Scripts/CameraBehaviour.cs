using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform frog;
    Vector3 target;
    public float smoothTime = 0.3F;     // Smoothing time
    public float LeadDistance = 10f;    // How far in front of the frog to stay
    private Vector3 velocity = Vector3.zero;

    void LateUpdate () {

        if (frog.position.x + LeadDistance > transform.position.x)
        {
            target = new Vector3(frog.position.x + LeadDistance, 0, -10);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        }

    }

}
