using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MinTongueLength = 2f;
    public float TongueRetractSpeed = 5f;
    public float TongueStrength = 1000f;

    private readonly List<Vector3> _swingPoints = new List<Vector3>();  // The point at which the tongue is attached.
    private bool _updateTongue = false;
    private LineRenderer _lineRenderer;

    private Vector3[] _linePoints;

    private class Rope
    {
        public float length { get; set; }
        public float spring_constant { get; set; }

        public Rope(float length, float spring_constant)
        {
            this.length = length;
            this.spring_constant = spring_constant;
        }

        public Rope(Vector2 one_end, Vector2 other_end, float spring_constant)
        {
            length = Vector2.SqrMagnitude(other_end - one_end);
            this.spring_constant = spring_constant;
        }

        public Vector2 Force(Vector3 this_end, Vector3 other_end)
        {
            Vector2 displacement = other_end - this_end;
            float distance = Vector2.SqrMagnitude(displacement);
            float extension = distance - length;

            if (extension > 0)
            {
                Vector2 direction = displacement / distance;
                return spring_constant*extension*direction;
            }
            return Vector2.zero;
        }
    }

    private Rope tongue;

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
                _swingPoints.Clear();
                _swingPoints.Add(hit.point);
                _updateTongue = true;

                // Cancel most velocity in direction away from swing point
                Vector2 force_dir = (hit.point - (Vector2) transform.position).normalized;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                float force_mag = Vector2.Dot(rb.velocity, -force_dir);
                rb.velocity += force_dir*force_mag;
            }
        }
        else if (Input.GetMouseButtonUp(0) && _swingPoints.Count > 0)
        {
            _swingPoints.Clear();
            _updateTongue = true;
        }

        // Check for collisions along tongue length
        if (_swingPoints.Count > 0)
        {
            hit = Physics2D.Linecast(transform.position, _swingPoints.Last(),
                1 << LayerMask.NameToLayer("Geometry"));
            if (hit.collider != null && Vector2.Distance(hit.point, _swingPoints.Last()) > 0.1)
            {
                _swingPoints.Add(hit.point);
                _updateTongue = true;
            }
        }

        // Check if any (last) connections are now unnecessary.
        if (_swingPoints.Count > 1)
        {
            hit = Physics2D.Linecast(transform.position, _swingPoints[_swingPoints.Count - 2],
                1 << LayerMask.NameToLayer("Geometry"));
            if (hit.collider == null || Vector2.Distance(hit.point, _swingPoints[_swingPoints.Count-2]) < 0.1)
            {
                _swingPoints.Remove(_swingPoints.Last());
                _updateTongue = true;
            }
        }

        // Recalculate lines and joints if necessary.
        if (_updateTongue)
        {
            tongue = null;
            if (_swingPoints.Count > 0)
            {
                tongue = new Rope(transform.position, _swingPoints.Last(), TongueStrength);
            }
        }

        // Decrease tongue length if the button is held down
        if (tongue != null && Input.GetMouseButton(0) && _swingPoints.Count > 0)
        {
            if (tongue.length > MinTongueLength)
            {
                tongue.length -= TongueRetractSpeed*Time.deltaTime;
            }
        }

        // Add force
        if (tongue != null)
        {
            GetComponent<Rigidbody2D>().AddForce(tongue.Force(transform.position, _swingPoints.Last()));
        }

        // Draw lines
        _lineRenderer.enabled = _swingPoints.Count > 0;

        if (_updateTongue)
        {
            _lineRenderer.positionCount = _swingPoints.Count;
            _lineRenderer.SetPositions(_swingPoints.ToArray());
            _lineRenderer.positionCount += 1;
        }
        _lineRenderer.SetPosition(_lineRenderer.positionCount-1, transform.position);

        if (_updateTongue)
        {
            _updateTongue = false;
        }
    }
}
