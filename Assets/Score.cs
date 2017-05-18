using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMesh MultiplierText;
    public GameObject EdgeEffect;
    public TextMesh ScoreText;

    public int EdgeBonus = 500;
    public float VelocityMultiplier = 0.5f; // How many points do you get per second for one unit of velocity?
    public float MultiplierVelocityThreshold = 1f;  // Score multiplier increases at integer multiples of this velocity.

    private Rigidbody2D rb;

    private int _score = 0;
    private int _multiplier = 0;

    private float _last_velocity_threshold = 0f;

    // User settings that we need to remember
    private float _multiplier_text_min_size;
    public float MultiplierTextMaxSize = 0.5f;
    public int MultiplierTextMaxSizeValue = 50;

    // Use this for initialization
    void Start ()
    {
        _score = 0;
        _multiplier = 0;
        rb = GetComponent<Rigidbody2D>();
        _multiplier_text_min_size = MultiplierText.characterSize;
    }
    
    // Update is called once per frame
    void Update () {
        // Edge bonus
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - transform.position, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Geometry"));
            if (hit.collider != null)
            {
                if (Mathf.Abs(hit.normal.y) < 0.001)
                {
                    mousePos.z = -1f;
                    Instantiate(EdgeEffect, mousePos, Quaternion.identity);
                    _score += _multiplier*EdgeBonus;
                }
            }
        }

        // Here's how the multiplier works:
        // When you pass integer multiples of the multiplier velocity threshold, your multiplier increases
        // If your speed decreases past the last threshold, multiplier is reset to 0.
        if (rb.velocity.magnitude < _last_velocity_threshold)
        {
            ResetMultiplier();
        }

        if (rb.velocity.magnitude > _last_velocity_threshold + MultiplierVelocityThreshold && rb.velocity.x > 0)
        {
            IncrementMultiplier();
        }

        // General score increase for moving right
        if (rb.velocity.x > 0)
        {
            _score += _multiplier*Mathf.FloorToInt(VelocityMultiplier*rb.velocity.magnitude*Time.deltaTime);
        }

        // Update score display
        ScoreText.text = _score.ToString();
    }

    private void ResetMultiplier()
    {
        _multiplier = 0;
        MultiplierText.text = "";
        MultiplierText.characterSize = _multiplier_text_min_size;
    }

    private void IncrementMultiplier()
    {
        _multiplier++;
        MultiplierText.text = _multiplier.ToString() + "x";
        MultiplierText.characterSize = Mathf.Lerp(_multiplier_text_min_size, MultiplierTextMaxSize,
            ((float) _multiplier)/MultiplierTextMaxSizeValue);
    }
}
