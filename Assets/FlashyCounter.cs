using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class FlashyCounter : MonoBehaviour
{
    public int StartCount = 0;
    public int Target = 0;
    public int Interval = 1000; // How often to flash
    public float Speed = 100f;  // Speed at which counter is incremented (/s)
    public float MaxTime = 5f; // Maximum amount of time that the counter can take
    public string Suffix = ""; // Suffix to append to counter
    public GameObject FlashEffect;

    private bool _running = false;
    private float _last;
    private float _current;
    private TextMesh _text;

    // Use this for initialization
    void Start ()
    {
        _current = StartCount;
        _last = StartCount;
        _text = GetComponent<TextMesh>();
        if ((Target - StartCount)/Speed > MaxTime)
        {
            Speed = (Target - StartCount)/MaxTime;
        }
    }

    public void Begin()
    {
        _running = true;
    }
    
    // Update is called once per frame
    void Update () {
        if (_running && _current < Target)
        {
            _last = _current;
            _current += Speed*Time.deltaTime;
            int display = Mathf.FloorToInt(_current);
            if (_current > Target)
                _current = Target;

            int thresholdOfInterest = Interval*(Mathf.FloorToInt(_current/Interval));
            bool passedThreshold = _last < thresholdOfInterest && _current > thresholdOfInterest;
            if (display == Target || passedThreshold)
            {
                Flash();
            }
            _text.text = display.ToString() + Suffix;
        }
        if (_running && Mathf.FloorToInt(_current) == Target)
        {
            _running = false;
        }
    }

    private void Flash()
    {
        TextMesh effect = Instantiate(FlashEffect, transform).GetComponent<TextMesh>();
        effect.characterSize = _text.characterSize;
        effect.fontSize = _text.fontSize;
        effect.anchor = _text.anchor;
        effect.GetComponent<TextDisappearEffect>().end_size = _text.characterSize + 0.1f;
        effect.transform.localPosition = Vector2.zero;
        effect.text = Mathf.FloorToInt(_current).ToString() + Suffix;
    }

    public bool IsRunning()
    {
        return _running;
    }
}
