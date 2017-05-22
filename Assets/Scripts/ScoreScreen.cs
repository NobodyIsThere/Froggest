using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    public FlashyCounter[] Counters;

    private bool _started = false;
    private int _currentCounter = 0;

    // Use this for initialization
    void Start () {
        
    }

    public void Begin(int[] values)
    {
        for (var i = 0; i < values.Length; i++)
        {
            Counters[i].Target = values[i];
        }
        Counters[0].Begin();
        _started = true;
    }
    
    // Update is called once per frame
    void Update () {
        if (_started && !Counters[_currentCounter].IsRunning())
        {
            _currentCounter++;
            if (_currentCounter < Counters.Length)
            {
                Counters[_currentCounter].Begin();
            }
            else
            {
                _started = false;
            }
        }
    }
}
