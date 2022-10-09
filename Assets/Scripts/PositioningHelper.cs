using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PositioningHelper
{
    private Transform _canvasTransform;
    private float _timerHeight;
    private float _halfNumOfTimers;
    private float _initialOffset;
    private float _margin;
    private AnimationSettings _animSettings;

    public PositioningHelper(Transform canvasTransform, float timerHeight, int numOfTimers, float margin,
        AnimationSettings animSettings)
    {
        _canvasTransform = canvasTransform;
        _timerHeight = timerHeight;
        ChangeTimersCount(numOfTimers);
        _margin = margin;
        _animSettings = animSettings;
    }

    public TimerButton InstantiateAndPositionNewTimer(TimerButton timerPrefab, int timerIndex)
    {
        var timerButton = Object.Instantiate(timerPrefab, _canvasTransform);
        timerButton.transform.localPosition = new Vector3(0f,
            -timerIndex * _timerHeight - _margin * timerIndex + _initialOffset, 0f);
        return timerButton;
    }

    public void RepositionButtons(List<TimerButton> buttons)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var button = buttons[i];
            var newY = -i * _timerHeight - _margin * i + _initialOffset;
            button.rectTransform
                .DOLocalMoveY(newY, _animSettings.genericTweenDuration, true)
                .SetEase(_animSettings.genericEasing);
        }
    }

    public void ChangeTimersCount(int numOfTimers)
    {
        _halfNumOfTimers = numOfTimers * 0.5f;
        _initialOffset = _halfNumOfTimers * _timerHeight;
    }
}