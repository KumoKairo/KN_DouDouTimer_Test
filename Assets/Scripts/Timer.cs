using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class Timer: ISerializationCallbackReceiver
{
    public event Action<int> OnTimerCompleted; 

    private string _timerUiValue;
    
    private TimeSpan _secondsLeft;
    private DateTime _dueAt;
    [SerializeField]
    private bool _isRunning;
    [SerializeField]
    private int _index;

    [SerializeField] private double _secondsLeftSerialized;
    [SerializeField] private string _dueAtSerialized;

    public Timer(float secondsLeft, int index)
    {
        _isRunning = false;
        _secondsLeft = TimeSpan.FromSeconds(secondsLeft);
        _dueAt = DateTime.UtcNow; // Serialization / Deserialization Exception safeguard
        _index = index;
        UpdateTimerUiValue();
    }

    public void Start()
    {
        _dueAt = DateTime.UtcNow.Add(_secondsLeft);
        if (_secondsLeft.TotalSeconds > 0f)
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

        _secondsLeft = _dueAt.Subtract(DateTime.UtcNow);
        if (_secondsLeft.TotalSeconds < 0f)
        {
            _secondsLeft = TimeSpan.Zero;
            _isRunning = false;

            OnTimerCompleted?.Invoke(_index);
        }
    }

    public string GetTimerUiValue()
    {
        UpdateTimerUiValue();
        return _timerUiValue;
    }
    
    public void ChangeTickValue(float value)
    {
        if (_isRunning)
        {
            _dueAt = _dueAt.AddSeconds(value);
        }
        else
        {
            _secondsLeft = _secondsLeft.Add(TimeSpan.FromSeconds(value));
            if (_secondsLeft.TotalSeconds < 0.0)
            {
                _secondsLeft = TimeSpan.Zero;
            }
        }
    }

    private void UpdateTimerUiValue()
    {
        _timerUiValue = $"{_secondsLeft:mm\\:ss\\:ff}";
    }

    // Serialization workaround / wrapper code
    // We need to store a global point in time and load it back when starting again
    public void OnBeforeSerialize()
    {
        _secondsLeftSerialized = _secondsLeft.TotalSeconds;
        // Losing milliseconds here, but it's more robust and simple than other solutions
        _dueAtSerialized = _dueAt.ToString(CultureInfo.InvariantCulture); 
    }

    public void OnAfterDeserialize()
    {
        _secondsLeft = TimeSpan.FromSeconds(_secondsLeftSerialized);
        _dueAt = DateTime.Parse(_dueAtSerialized, CultureInfo.InvariantCulture);
    }
}