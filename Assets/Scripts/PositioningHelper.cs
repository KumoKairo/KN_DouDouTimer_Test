using System.Collections.Generic;
using UnityEngine;

public class PositioningHelper
{
    private Transform _canvasTransform;
    private float _timerHeight;
    private float _halfNumOfTimers;
    private float _initialOffset;
    private float _margin;

    public PositioningHelper(Transform canvasTransform, float timerHeight, int numOfTimers, float margin)
    {
        _canvasTransform = canvasTransform;
        _timerHeight = timerHeight;
        ChangeTimersCount(numOfTimers);
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

    public void RepositionButtons(List<TimerButton> buttons)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];
            button.transform.localPosition = new Vector3(0f,
                -i * _timerHeight - _margin * i + _initialOffset, 0f);
        }
    }

    public void ChangeTimersCount(int numOfTimers)
    {
        _halfNumOfTimers = numOfTimers * 0.5f;
        _initialOffset = _halfNumOfTimers * _timerHeight;
    }
}