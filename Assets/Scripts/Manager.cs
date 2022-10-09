using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private const int MaxNumberOfTimers = 6;

    public float defaultTimerDurationSeconds = 90f;

    [Space] public Canvas rootCanvas;
    public TimerPanel timerPanel;

    public CanvasGroup controlButtonsGroup;
    public Button addTimerButton;
    public Button removeTimerButton;

    public TimerButton timerButtonPrefab;
    public AnimationSettings animSettings;

    private List<TimerButton> _timerButtons;
    private List<Timer> _timers;
    private PersistenceLayer _persistenceLayer;
    private PositioningHelper _positioningHelper;
    private int _numberOfTimers;

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

        controlButtonsGroup.blocksRaycasts = false;
        controlButtonsGroup.alpha = 0f;

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
                var newTimer = new Timer(defaultTimerDurationSeconds, i);
                _timers.Add(newTimer);
            }

            var timer = _timers[i];
            timer.OnTimerCompleted += OnTimerCompleted;
        }

        controlButtonsGroup.DOFade(1f, animSettings.genericTweenDuration)
            .OnComplete(() => { controlButtonsGroup.blocksRaycasts = true; });
        CheckAddRemoveButtons();
    }

    private void OnTimerCompleted(int timerIndex)
    {
        _timerButtons[timerIndex].PlayCompletedAnimation();
    }

    public void OnTimerClicked(int index)
    {
        controlButtonsGroup.blocksRaycasts = false;
        controlButtonsGroup.DOFade(0f, animSettings.genericTweenDuration)
            .OnComplete(() => { controlButtonsGroup.gameObject.SetActive(false); });
        
        HideTimers();
        timerPanel.ShowForTimer(_timers[index]);
    }

    public void BackToTimerButtons()
    {
        timerPanel.Hide();
        ShowTimers();
        
        controlButtonsGroup.gameObject.SetActive(true);
        controlButtonsGroup.DOFade(1f, animSettings.genericTweenDuration)
            .OnComplete(() => { controlButtonsGroup.blocksRaycasts = true; });
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
        var timer = new Timer(defaultTimerDurationSeconds, timerIndex);
        timer.OnTimerCompleted += OnTimerCompleted;

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
        var timer = _timers[removingAtIndex];
        timer.OnTimerCompleted -= OnTimerCompleted;

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