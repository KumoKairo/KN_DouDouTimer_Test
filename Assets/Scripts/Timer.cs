using System;
using UnityEngine;

public class Timer
{
    private string _timerUiValue;
    private float _secondsLeft;
    private float _dueAt;
    private bool _isRunning;

    public Timer(float secondsLeft)
    {
        _isRunning = false;
        _secondsLeft = secondsLeft;
        UpdateTimerUiValue();
    }

    public void Start()
    {
        _dueAt = Time.unscaledTime + _secondsLeft;
        if (_secondsLeft > 0f)
        {
            _isRunning = true;
        }
    }

    public void Update()
    {
        if (!_isRunning)
        {
            return;
        }

        _secondsLeft = _dueAt - Time.unscaledTime;
        if (_secondsLeft < 0f)
        {
            _secondsLeft = 0f;
            _isRunning = false;
        }
    }

    public string GetTimerUiValue()
    {
        UpdateTimerUiValue();
        return _timerUiValue;
    }

    private void UpdateTimerUiValue()
    {
        _timerUiValue = $"{TimeSpan.FromSeconds(_secondsLeft):mm\\:ss\\:ff}";
    }
}