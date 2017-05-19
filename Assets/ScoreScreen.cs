using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    public FlashyCounter[] Counters;

    private bool _started = false;
    private int _current_counter = 0;

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
        if (_started && !Counters[_current_counter].IsRunning())
        {
            if (_current_counter++ < Counters.Length)
            {
                Counters[_current_counter].Begin();
            }
            else
            {
                _started = false;
            }
        }
    }
}
