using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Effects
    public Text MultiplierText;
    public GameObject TextFlashEffect;
    public GameObject EdgeEffect;
    public GameObject PickupEffect;
    public Text ScoreText;

    public GameObject scoreScreen;

    // Parameters
    public int EdgeBonus = 500;
    public int PickupScore = 1000;
    public float VelocityMultiplier = 10f; // How many points do you get per second for one unit of velocity?
    public float MultiplierVelocityThreshold = 5f;  // Score multiplier increases at integer multiples of this velocity.

    private Rigidbody2D rb;

    // Things we're keeping track of
    private int _score = 0;
    private int _multiplier = 0;
    private int _max_multiplier = 0;
    private int _num_flies = 0;
    private int _num_edges = 0;

    private int _finalScore = 0;
    private int _finalDistance = 0;

    private float _last_velocity_threshold = 0f;
    private Transform _last_platform = null;

    // User settings that we need to remember
    private float _multiplier_text_min_size;
    public float MultiplierTextMaxSize = 150f;
    public int MultiplierTextMaxSizeValue = 50;

    // Stuff that we work out at Start time.
    private int _geometryLayer = 0;
    private int _pickupsLayer = 0;
    private PlayerMovement _playerMovement;

    // Memory allocation
    private int _numHits = 0;
    private RaycastHit2D[] _tongueHits = new RaycastHit2D[2];

    // Use this for initialization
    void Start ()
    {
        _score = 0;
        _multiplier = 0;
        rb = GetComponent<Rigidbody2D>();
        _multiplier_text_min_size = MultiplierText.fontSize;
        _geometryLayer = LayerMask.NameToLayer("Geometry");
        _pickupsLayer = LayerMask.NameToLayer("Pickups");
        _playerMovement = GetComponent<PlayerMovement>();
    }
    
    // Update is called once per frame
    void Update () {
        // Edge bonus
        if (Input.GetMouseButtonDown(0) || _playerMovement.HasJustFakeClicked())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_playerMovement.HasJustFakeClicked())
            {
                mousePos = _playerMovement.FakeClickPoint();
            }
            mousePos.z = 0;
            _numHits = Physics2D.RaycastNonAlloc(transform.position, mousePos - transform.position, _tongueHits,
                float.PositiveInfinity, (1 << _geometryLayer) | (1 << _pickupsLayer));
            if (_numHits > 0)
            {
                // Geometry
                if (_tongueHits[0].transform.gameObject.layer == _geometryLayer && _tongueHits[0].transform != _last_platform)
                {
                    if (Mathf.Abs(_tongueHits[0].normal.y) < 0.001)
                    {
                        mousePos.z = -1f;
                        if (_multiplier > 0)
                        {
                            int increment = _multiplier * EdgeBonus;
                            GameObject edge_effect = Instantiate(EdgeEffect, mousePos, Quaternion.identity);
                            edge_effect.transform.Find("PickupText").GetComponent<Text>().text = "+" + increment.ToString();
                            _num_edges++;
                            _score += increment;
                        }
                    }
                    if (_multiplier > 3)
                        IncrementMultiplier();
                    _last_platform = _tongueHits[0].transform;
                }
                // Flies
                else if (_tongueHits[0].transform.gameObject.layer == _pickupsLayer)
                {
                    GameObject pickupScore = Instantiate(PickupEffect, _tongueHits[0].transform.position, Quaternion.identity);
                    int increment = _multiplier * PickupScore;
                    pickupScore.GetComponentInChildren<Text>().text = "+" + increment.ToString();
                    _score += increment;
                    Destroy(_tongueHits[0].transform.gameObject);
                    _num_flies++;
                }
            }
        }

        // Here's how the multiplier works:
        // When you pass integer multiples of the multiplier velocity threshold, your multiplier increases
        // If your speed decreases past the last threshold, multiplier is reset to 0.
        if (rb.velocity.magnitude < _last_velocity_threshold && OnScreen())
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
        MultiplierText.fontSize = (int)_multiplier_text_min_size;

        _last_velocity_threshold = 0;
    }

    private void IncrementMultiplier()
    {
        _multiplier++;
        MultiplierText.text = _multiplier.ToString() + "x";
        MultiplierText.fontSize = (int) Mathf.Lerp(_multiplier_text_min_size, MultiplierTextMaxSize,
            ((float) _multiplier)/MultiplierTextMaxSizeValue);
        Instantiate(TextFlashEffect, MultiplierText.transform);

        _last_velocity_threshold = rb.velocity.magnitude - 0.5f*MultiplierVelocityThreshold;

        if (_multiplier > _max_multiplier)
        {
            _max_multiplier = _multiplier;
        }
    }

    private bool OnScreen()
    {
        return transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;
    }

    public void EndGame()
    {
        rb.isKinematic = true;

        _finalDistance = (int) rb.position.x;
        _finalScore = _score;

        rb.position = transform.position;
        rb.velocity = Vector2.zero;

        ScoreScreen ss = Instantiate(scoreScreen, Camera.main.transform).GetComponent<ScoreScreen>();
        ss.gameObject.transform.localPosition = new Vector3(0, 0, 1);
        ss.Begin(new int[] {_finalDistance, _finalScore, _max_multiplier, _num_flies, _num_edges});
    }

    public int GetFinalScore()
    {
        return _finalScore;
    }

    public int GetFinalDistance()
    {
        return _finalDistance;
    }

    public int GetMaxMultiplier()
    {
        return _max_multiplier;
    }

    public int GetFliesEaten()
    {
        return _num_flies;
    }

    public int GetNumEdges()
    {
        return _num_edges;
    }
}
