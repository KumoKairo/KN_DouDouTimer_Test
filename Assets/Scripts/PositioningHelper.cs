using UnityEngine;

public class PositioningHelper
{
    private Transform _canvasTransform;
    private float _timerHeight;
    private int _floorHalfNumOfTimers;
    private float _initialOffset;
    private float _margin;

    public PositioningHelper(Transform canvasTransform, float timerHeight, int numOfTimers, float margin)
    {
        this._canvasTransform = canvasTransform;
        _timerHeight = timerHeight;
        _floorHalfNumOfTimers = numOfTimers / 2;
        _initialOffset = _floorHalfNumOfTimers * (timerHeight + _floorHalfNumOfTimers * margin);
        _margin = margin;
    }

    public TimerButton InstantiateAndPositionNewTimer(TimerButton timerPrefab, int timerIndex, Manager manager)
    {
        var timerButton = Object.Instantiate(timerPrefab, _canvasTransform);
        timerButton.transform.localPosition = new Vector3(0f,
            -timerIndex * _timerHeight - _margin * timerIndex + _initialOffset, 0f);
        timerButton.Init(manager, timerIndex);
        return timerButton;
    }
}
