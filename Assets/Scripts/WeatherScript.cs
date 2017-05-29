using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WeatherScript : MonoBehaviour
{
    public enum Weather { CLEAR, RAIN, SNOW };

    public float WeatherChangeDistance = 500f;  // Weather changes after multiples of this distance.

    public ParticleSystem RainSystem;
    public ParticleSystem SnowSystem;
    public PlatformManager PM;
    public Rigidbody2D Player;
    public Color RainySkyColour;
    public Color SnowySkyColour;
    public float SkyFadeTime = 1f;

    private Weather _weather;

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
            switch (_weather)
            {
                case Weather.RAIN:
                    StartSnow();
                    break;
                case Weather.SNOW:
                    StartClear();
                    break;
                case Weather.CLEAR:
                    StartRain();
                    break;
                default:
                    StartClear();
                    break;
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
        ChangeSky(RainySkyColour);
        _weather = Weather.RAIN;
        PM.Weather = _weather;
    }

    private void StartSnow()
    {
        RainSystem.Stop();
        SnowSystem.Play();
        ChangeSky(SnowySkyColour);
        _weather = Weather.SNOW;
        PM.Weather = _weather;
    }

    private void StartClear()
    {
        RainSystem.Stop();
        SnowSystem.Stop();
        ChangeSky(_normalColour);
        _weather = Weather.CLEAR;
        PM.Weather = _weather;
    }
}
