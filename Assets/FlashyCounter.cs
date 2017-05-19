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
    public GameObject FlashEffect;

    private bool _running = false;
    private int _current;
    private TextMesh _text;

    // Use this for initialization
    void Start ()
    {
        _current = StartCount;
        _text = GetComponent<TextMesh>();
    }

    public void Begin()
    {
        _running = true;
    }
    
    // Update is called once per frame
    void Update () {
        if (_running && _current < Target)
        {
            _current += (int) (Speed*Time.deltaTime);
            if (_current > Target)
                _current = Target;

            if (_current == Target || _current%Interval == 0)
            {
                Flash();
            }
            _text.text = _current.ToString();
        }
        if (_current == Target)
        {
            _running = false;
        }
    }

    private void Flash()
    {
        TextMesh effect = Instantiate(FlashEffect, transform).GetComponent<TextMesh>();
        effect.text = _current.ToString();
    }

    public bool IsRunning()
    {
        return _running;
    }
}
