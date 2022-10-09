using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private const int MaxNumberOfTimers = 6;

    public float defaultTimerDurationSeconds = 90f;

    [Space] public Canvas rootCanvas;
    public TimerPanel timerPanel;
    public Button addTimerButton;
    public Button removeTimerButton;

    public TimerButton timerButtonPrefab;
    public AnimationSettings animSettings;

    private List<TimerButton> _timerButtons;
    private List<Timer> _timers;
    private PersistenceLayer _persistenceLayer;
    private PositioningHelper _positioningHelper;
    private int _numberOfTimers;

    private bool _hasInitialized;

    private void Start()
    {
        const int defaultNumOfTimers = 3;
        const float margin = 8f;

        _numberOfTimers = defaultNumOfTimers;
        _persistenceLayer = new PersistenceLayer();
        var savedTimers = _persistenceLayer.TryLoad();

        if (savedTimers != null)
        {
            _numberOfTimers = savedTimers.Count;
        }

        _timerButtons = new List<TimerButton>(_numberOfTimers);
        _timers = savedTimers ?? new List<Timer>(_numberOfTimers);
        _positioningHelper = new PositioningHelper(rootCanvas.transform, timerButtonPrefab.rectTransform.rect.height,
            _numberOfTimers, margin, animSettings);

        for (int i = 0; i < _numberOfTimers; i++)
        {
            var timerButton = _positioningHelper.InstantiateAndPositionNewTimer(timerButtonPrefab, i);
            timerButton.Init(this, i);
            timerButton.TweenInFromSide(this);

            _timerButtons.Add(timerButton);
            if (savedTimers == null)
            {
                var timer = new Timer(defaultTimerDurationSeconds);
                _timers.Add(timer);
            }
        }

        CheckAddRemoveButtons();

        _hasInitialized = true;
    }

    public void OnTimerClicked(int index)
    {
        HideTimers();
        timerPanel.ShowForTimer(_timers[index]);
    }

    public void BackToTimerButtons()
    {
        timerPanel.Hide();
        ShowTimers();
    }

    public void OnAddTimer()
    {
        if (_numberOfTimers >= MaxNumberOfTimers)
        {
            return;
        }

        _numberOfTimers++;
        var timerIndex = _numberOfTimers - 1;
        var timerButton = _positioningHelper.InstantiateAndPositionNewTimer(timerButtonPrefab, timerIndex);
        timerButton.Init(this, timerIndex);
        timerButton.TweenInFromBottom();

        _timerButtons.Add(timerButton);
        var timer = new Timer(defaultTimerDurationSeconds);
        _timers.Add(timer);

        _positioningHelper.ChangeTimersCount(_numberOfTimers);
        _positioningHelper.RepositionButtons(_timerButtons);

        CheckAddRemoveButtons();
    }

    public void OnRemoveTimer()
    {
        if (_timers.Count <= 1)
        {
            return;
        }

        var removingAtIndex = _numberOfTimers - 1;
        _timers.RemoveAt(removingAtIndex);
        var timerButton = _timerButtons[removingAtIndex];
        _timerButtons.RemoveAt(removingAtIndex);
        timerButton.EaseOutDestroy();

        _numberOfTimers--;
        _positioningHelper.ChangeTimersCount(_numberOfTimers);
        _positioningHelper.RepositionButtons(_timerButtons);

        CheckAddRemoveButtons();
    }

    private void CheckAddRemoveButtons()
    {
        addTimerButton.interactable = _numberOfTimers < MaxNumberOfTimers;
        removeTimerButton.interactable = _numberOfTimers > 1;
    }

    private void HideTimers()
    {
        for (int i = 0; i < _timerButtons.Count; i++)
        {
            _timerButtons[i].DisableAndHide();
        }
    }

    private void ShowTimers()
    {
        for (int i = 0; i < _timerButtons.Count; i++)
        {
            _timerButtons[i].EnableAndShow();
        }
    }

    private void Update()
    {
        if (!_hasInitialized)
        {
            return;
        }

        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i].Update();
        }
    }

    private void OnDestroy()
    {
        _persistenceLayer.Save(_timers);
    }
}