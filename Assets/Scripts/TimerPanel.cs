using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerPanel : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Manager manager;

    public Button[] buttons;

    private Timer _currentTimer;
    
    public void ShowForTimer(Timer timer)
    {
        _currentTimer = timer;
        gameObject.SetActive(true);
        UpdateTimerText();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnIncreaseTimer()
    {
        _currentTimer.Increase();
    }

    public void OnDecreaseTimer()
    {
        _currentTimer.Decrease();
    }

    public void OnStartTimer()
    {
        _currentTimer.Start();
        manager.BackToTimerButtons();
    }

    private void UpdateTimerText()
    {
        timerText.text = _currentTimer.GetTimerUiValue();
    }

    private void Update()
    {
        UpdateTimerText();
    }
}