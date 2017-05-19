using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMesh MultiplierText;
    public GameObject MultiplierTextEffect;
    public GameObject EdgeEffect;
    public GameObject PickupEffect;
    public TextMesh ScoreText;

    public GameObject scoreScreen;

    public int EdgeBonus = 500;
    public int PickupScore = 1000;
    public float VelocityMultiplier = 10f; // How many points do you get per second for one unit of velocity?
    public float MultiplierVelocityThreshold = 5f;  // Score multiplier increases at integer multiples of this velocity.

    private Rigidbody2D rb;

    private int _score = 0;
    private int _multiplier = 0;

    private float _last_velocity_threshold = 0f;

    // User settings that we need to remember
    private float _multiplier_text_min_size;
    public float MultiplierTextMaxSize = 0.5f;
    public int MultiplierTextMaxSizeValue = 20;

    // Stuff that we work out at Start time.
    private int _geometryLayer = 0;
    private int _pickupsLayer = 0;
    private PlayerMovement _playerMovement;

    // Use this for initialization
    void Start ()
    {
        _score = 0;
        _multiplier = 0;
        rb = GetComponent<Rigidbody2D>();
        _multiplier_text_min_size = MultiplierText.characterSize;
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - transform.position,
                float.PositiveInfinity, (1 << _geometryLayer) | (1 << _pickupsLayer));
            if (hit.collider != null)
            {
                // Geometry
                if (hit.transform.gameObject.layer == _geometryLayer)
                {
                    if (Mathf.Abs(hit.normal.y) < 0.001)
                    {
                        mousePos.z = -1f;
                        Instantiate(EdgeEffect, mousePos, Quaternion.identity);
                        _score += _multiplier*EdgeBonus;
                    }
                    if (_multiplier > 3)
                        IncrementMultiplier();
                }
                else if (hit.transform.gameObject.layer == _pickupsLayer)
                {
                    GameObject pickupScore = Instantiate(PickupEffect, hit.transform.position, Quaternion.identity);
                    pickupScore.GetComponent<TextMesh>().text = PickupScore.ToString();
                    _score += _multiplier*PickupScore;
                    Destroy(hit.transform.gameObject);
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
        MultiplierText.characterSize = _multiplier_text_min_size;

        _last_velocity_threshold = 0;
    }

    private void IncrementMultiplier()
    {
        _multiplier++;
        MultiplierText.text = _multiplier.ToString() + "x";
        MultiplierText.characterSize = Mathf.Lerp(_multiplier_text_min_size, MultiplierTextMaxSize,
            ((float) _multiplier)/MultiplierTextMaxSizeValue);
        GameObject effect_obj = Instantiate(MultiplierTextEffect, MultiplierText.transform);
        effect_obj.transform.localPosition = Vector3.zero;
        effect_obj.GetComponent<TextMesh>().text = _multiplier.ToString() + "x";
        effect_obj.GetComponent<TextMesh>().characterSize = MultiplierText.characterSize;
        TextDisappearEffect effect = effect_obj.AddComponent<TextDisappearEffect>();
        effect.start_size = MultiplierText.characterSize;
        effect.end_size = MultiplierTextMaxSize + 0.05f;

        _last_velocity_threshold = rb.velocity.magnitude - 0.5f*MultiplierVelocityThreshold;
    }

    private bool OnScreen()
    {
        return transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).y;
    }

    public void EndGame()
    {
        rb.isKinematic = true;

        int finalDistance = (int) rb.position.x;
        int finalScore = _score;

        rb.position = transform.position;
        rb.velocity = Vector2.zero;

        ScoreScreen ss = Instantiate(scoreScreen, Camera.main.transform).GetComponent<ScoreScreen>();
        ss.gameObject.transform.localPosition = new Vector3(0, 0, 1);
        ss.Begin(new int[] {finalDistance, finalScore});
    }
}
