using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float TongueRetractSpeed = 3f;

    private List<Vector3> swing_points = new List<Vector3>();  // The point at which the tongue is attached.
    private bool _updateTongue = false;
    private LineRenderer _lineRenderer;
    
    // Use this for initialization
    void Start ()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update () {
        RaycastHit2D hit;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            hit = Physics2D.Raycast(transform.position, mousePos - transform.position, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Geometry"));
            if (hit.collider != null)
            {
                swing_points.Clear();
                swing_points.Add(hit.point);
                _updateTongue = true;
            }
        }
        else if (Input.GetMouseButtonUp(0) && swing_points.Count > 0)
        {
            swing_points.Clear();
            _updateTongue = true;
        }

        // Check for collisions along tongue length
        if (swing_points.Count > 0)
        {
            hit = Physics2D.Linecast(transform.position, swing_points.Last(),
                1 << LayerMask.NameToLayer("Geometry"));
            if (hit.collider != null && Vector2.Distance(hit.point, swing_points.Last()) > 0.1)
            {
                swing_points.Add(hit.point);
                _updateTongue = true;
            }
        }

        // Check if any (last) connections are now unnecessary.
        if (swing_points.Count > 1)
        {
            hit = Physics2D.Linecast(transform.position, swing_points[swing_points.Count - 2],
                1 << LayerMask.NameToLayer("Geometry"));
            if (hit.collider == null || Vector2.Distance(hit.point, swing_points[swing_points.Count-2]) < 0.1)
            {
                swing_points.Remove(swing_points.Last());
                _updateTongue = true;
            }
        }

        // Recalculate lines and joints if necessary.
        if (_updateTongue)
        {
            Destroy(gameObject.GetComponent<SpringJoint2D>());
            if (swing_points.Count > 0)
            {
                SpringJoint2D swingPart = gameObject.AddComponent<SpringJoint2D>();
                swingPart.autoConfigureDistance = true;
                swingPart.connectedAnchor = swing_points.Last();
            }
            _updateTongue = false;
        }

        // Decrease tongue length if the button is held down
        if (Input.GetMouseButton(0) && swing_points.Count > 0)
        {
            SpringJoint2D swingPart = GetComponent<SpringJoint2D>();
            swingPart.distance -= TongueRetractSpeed*Time.deltaTime;
        }

        // Draw lines
        _lineRenderer.enabled = swing_points.Count > 0;
        _lineRenderer.numPositions = swing_points.Count;
        _lineRenderer.SetPositions(swing_points.ToArray());
        _lineRenderer.numPositions += 1;
        _lineRenderer.SetPosition(_lineRenderer.numPositions-1, transform.position);
    }
}
