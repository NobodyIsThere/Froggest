using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Configurable parameters
    public float MinTongueLength = 2f;
    public float TongueRetractSpeed = 5f;
    public float TongueStrength = 1000f;

    // Tongue physics
    private readonly List<Vector3> _swingPoints = new List<Vector3>();  // The point at which the tongue is attached.
    private bool _updateTongue = false;
    private LineRenderer _lineRenderer;
    private Vector3[] _linePoints;

    // Fake input
    private bool _is_fake_clicking = false;
    private bool _has_just_fake_clicked = false;
    private bool _has_just_fake_released = false;
    private Vector2 _fake_click_point;

    // Memory allocation
    private int _numHits = 0;
    private RaycastHit2D[] _tongueHits = new RaycastHit2D[2];

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

    public void Click(Vector2 screenPos)
    {
        _is_fake_clicking = true;
        _has_just_fake_clicked = true;
        _fake_click_point = screenPos;
    }

    public void Release()
    {
        _is_fake_clicking = false;
        _has_just_fake_released = true;
    }

    public bool IsFakeClicking()
    {
        return _is_fake_clicking;
    }

    public Vector2 FakeClickPoint()
    {
        return _fake_click_point;
    }

    public bool HasJustFakeClicked()
    {
        return _has_just_fake_clicked;
    }

    public bool HasJustFakeReleased()
    {
        return _has_just_fake_released;
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) || _has_just_fake_clicked)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_has_just_fake_clicked)
            {
                mousePos = Camera.main.ScreenToWorldPoint(_fake_click_point);
            }
            mousePos.z = 0;
            _numHits = Physics2D.RaycastNonAlloc(transform.position, mousePos - transform.position, _tongueHits, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Geometry"));
            if (_numHits > 0)
            {
                _swingPoints.Clear();
                _swingPoints.Add(_tongueHits[0].point);
                gameObject.GetComponent<Logger>().registerMouseCoordinates(_tongueHits[0].point); // log mouse coords
                _updateTongue = true;

                // Cancel most velocity in direction away from swing point
                Vector2 forceDir = (_tongueHits[0].point - (Vector2) transform.position).normalized;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                float forceMag = Vector2.Dot(rb.velocity, -forceDir);
                rb.velocity += forceDir*forceMag;
            }
        }
        else if ((Input.GetMouseButtonUp(0) || _has_just_fake_released) && _swingPoints.Count > 0)
        {
            _swingPoints.Clear();
            _updateTongue = true;
        }

        // Check for collisions along tongue length
        if (_swingPoints.Count > 0)
        {
            _numHits = Physics2D.LinecastNonAlloc(transform.position, _swingPoints.Last(), _tongueHits,
                1 << LayerMask.NameToLayer("Geometry"));
            if (_numHits > 0 && Vector2.Distance(_tongueHits[0].point, _swingPoints.Last()) > 0.1)
            {
                _swingPoints.Add(_tongueHits[0].point);
                _updateTongue = true;
            }
        }

        // Check if any (last) connections are now unnecessary.
        if (_swingPoints.Count > 1)
        {
            _numHits = Physics2D.LinecastNonAlloc(transform.position, _swingPoints[_swingPoints.Count - 2], _tongueHits,
                1 << LayerMask.NameToLayer("Geometry"));
            if (_numHits == 0 || Vector2.Distance(_tongueHits[0].point, _swingPoints[_swingPoints.Count-2]) < 0.1)
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
        if (tongue != null && (Input.GetMouseButton(0) || _is_fake_clicking) && _swingPoints.Count > 0)
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

        _has_just_fake_clicked = false;
        _has_just_fake_released = false;
    }
}
