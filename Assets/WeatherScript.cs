using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WeatherScript : MonoBehaviour
{

    public float WeatherChangeDistance = 500f;  // Weather changes after multiples of this distance.

    public ParticleSystem RainSystem;
    public ParticleSystem SnowSystem;
    public GameObject PlatformSpawner;
    public Rigidbody2D Player;
    public Color RainySkyColour;
    public Color SnowySkyColour;
    public float SkyFadeTime = 1f;

    private bool _isRaining = false;
    private bool _isSnowing = false;

    private float _lastThreshold = 0f;
    private Camera _cam;
    private Color _normalColour;
    private Color _startColour;
    private Color _targetColour;
    private bool _isSkyChangingColour = false;
    private float _fadeStartTime;

    // Use this for initialization
    void Start () {
        _cam = Camera.main;
        _normalColour = _cam.backgroundColor;
        StartClear();
    }
    
    // Update is called once per frame
    void Update () {
        if (_isSkyChangingColour)
        {
            float t = (Time.time - _fadeStartTime)/SkyFadeTime;
            _cam.backgroundColor = Color.Lerp(_startColour, _targetColour, t);
            if (t > 1f)
                _isSkyChangingColour = false;
        }

        if (Player.position.x > _lastThreshold + WeatherChangeDistance)
        {
            _lastThreshold += WeatherChangeDistance;
            if (_isRaining)
            {
                StartSnow();
            }
            else if (_isSnowing)
            {
                StartClear();
            }
            else if (!_isRaining)
            {
                StartRain();
            }
        }
    }

    private void ChangeSky(Color colour)
    {
        _isSkyChangingColour = true;
        _startColour = _cam.backgroundColor;
        _targetColour = colour;
        _fadeStartTime = Time.time;
    }

    private void StartRain()
    {
        RainSystem.Play();
        SnowSystem.Stop();
        PlatformSpawner.GetComponent<PlatformManager>().IsRaining = true;
        ChangeSky(RainySkyColour);
        _isRaining = true;
        _isSnowing = false;
    }

    private void StartSnow()
    {
        RainSystem.Stop();
        SnowSystem.Play();
        PlatformSpawner.GetComponent<PlatformManager>().IsRaining = false;
        PlatformSpawner.GetComponent<PlatformManager>().IsSnowing = true;
        ChangeSky(SnowySkyColour);
        _isRaining = false;
        _isSnowing = true;
    }

    private void StartClear()
    {
        RainSystem.Stop();
        SnowSystem.Stop();
        PlatformSpawner.GetComponent<PlatformManager>().IsRaining = false;
        PlatformSpawner.GetComponent<PlatformManager>().IsSnowing = false;
        ChangeSky(_normalColour);
        _isRaining = false;
        _isSnowing = false;
    }
}
