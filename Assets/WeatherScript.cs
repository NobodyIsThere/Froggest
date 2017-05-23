using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherScript : MonoBehaviour {

    public float RainDistance = 100f;

    public GameObject RainSystem;
    public GameObject PlatformSpawner;
    public Rigidbody2D player;
    public Color RainySkyColour;
    public float SkyFadeTime = 1f;

    private bool _isRaining = false;

    private Camera cam;
    private Color _normal_colour;
    private Color _start_colour;
    private Color _target_colour;
    private bool _is_sky_changing_colour = false;
    private float _fade_start_time;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
    }
    
    // Update is called once per frame
    void Update () {
        if (_is_sky_changing_colour)
        {
            float t = (Time.time - _fade_start_time)/SkyFadeTime;
            cam.backgroundColor = Color.Lerp(_start_colour, _target_colour, t);
            if (t > 1f)
                _is_sky_changing_colour = false;
        }

        if (!_isRaining && player.position.x > RainDistance)
        {
            StartRain();
        }
    }

    private void StartRain()
    {
        RainSystem.SetActive(true);
        PlatformSpawner.GetComponent<PlatformManager>().IsRaining = true;
        _is_sky_changing_colour = true;
        _start_colour = cam.backgroundColor;
        _target_colour = RainySkyColour;
        _fade_start_time = Time.time;
        _isRaining = true;
    }
}
