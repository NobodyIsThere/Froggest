﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSonic : MonoBehaviour {

	public float ReleaseAngle = 0.25f * Mathf.PI;
	public float ReleaseDistance = 1f;
	public float ReleaseSpeed = 0.01f;
	public float AttachVertSpeed = 0.1f; // If vspeed below this, look for new platform.
	public float AngleResolution = 0.0625f * Mathf.PI;

	private bool _isAttached;
	private Vector2 _attachedPoint;

	// Things to grab at startup
	private Rigidbody2D rb;
	private PlayerMovement movement;
	private int _geometryLayer;
	private float _lowerLimit;

	// Memory allocation
	private RaycastHit2D[] _hits = new RaycastHit2D[1];

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		movement = GetComponent<PlayerMovement> ();
		_geometryLayer = LayerMask.NameToLayer ("Geometry");
		_lowerLimit = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane)).y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If we are swinging,
		// 	If direction is above ReleaseAngle, release
		//	If we are past ReleaseDistance past the anchor, release
		//  If we are moving too slowly, release
		// Else
		//	If vertical speed is below AttachVertSpeed, look for new platform
		if (_isAttached)
		{
			if (Mathf.Atan2 (rb.velocity.y, rb.velocity.x) > ReleaseAngle ||
			    rb.position.x > _attachedPoint.x + ReleaseDistance ||
				rb.velocity.magnitude < ReleaseSpeed)
			{
				Release ();
			}
		}
		else
		{
			if (rb.velocity.y < AttachVertSpeed)
			{
				Vector2 best_position = rb.position;
				for (float angle = -0.5f * Mathf.PI; angle < 0.5f * Mathf.PI; angle += AngleResolution)
				{
					int num_hits = Physics2D.RaycastNonAlloc (rb.position, AngleToVector (angle), _hits, Mathf.Infinity, 1 << _geometryLayer);
					if (num_hits > 0)
					{
						if (_hits [0].point.x > best_position.x && Vector2.Distance (rb.position, _hits [0].point) < _hits [0].point.y - _lowerLimit)
						{
							best_position = _hits [0].point;
						}
					}
				}
				Attach (best_position);
			}
		}
	}

	private void Release()
	{
		movement.Release ();
		_isAttached = false;
	}

	private void Attach(Vector2 point)
	{
		movement.Click (Camera.main.WorldToScreenPoint (point));
		_isAttached = true;
		_attachedPoint = point;
	}

	private Vector2 AngleToVector(float angle)
	{
		return new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
	}
}
