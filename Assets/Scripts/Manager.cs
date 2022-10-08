using System;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public float defaultTimerDurationSeconds = 90f;

    [Space] public Canvas rootCanvas;
    public TimerPanel timerPanel;

    public TimerButton timerButtonPrefab;

    private List<TimerButton> _timerButtons;
    private List<Timer> _timers;
    private PersistenceLayer _persistenceLayer;

    private void Start()
    {
        const int numOfTimers = 4;
        const float margin = 8f;

        _persistenceLayer = new PersistenceLayer();
        var savedTimers = _persistenceLayer.TryLoad();
        
        _timerButtons = new List<TimerButton>(numOfTimers);
        _timers = new List<Timer>(numOfTimers);
        var canvasTransform = rootCanvas.transform;
        var timerHeight = timerButtonPrefab.rectTransform.rect.height;
        int floorHalf = numOfTimers / 2;
        var initialOffset = floorHalf * (timerHeight + floorHalf * margin);
        for (int i = 0; i < numOfTimers; i++)
        {
            var timerButton = Instantiate(timerButtonPrefab, canvasTransform);
            timerButton.transform.localPosition = new Vector3(0f, -i * timerHeight - margin * i + initialOffset, 0f);
            timerButton.Init(this, i);
            _timerButtons.Add(timerButton);
            var timer = savedTimers != null ? savedTimers[i] : new Timer(defaultTimerDurationSeconds);
            _timers.Add(timer);
        }
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