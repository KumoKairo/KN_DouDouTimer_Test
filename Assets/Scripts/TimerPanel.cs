using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerPanel : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Manager manager;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public AnimationSettings animSettings;

    private Timer _currentTimer;
    
    public void ShowForTimer(Timer timer)
    {
        _currentTimer = timer;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        var localPos = rectTransform.localPosition;
        localPos.x = animSettings.timerButtonPositionOffset;
        rectTransform.localPosition = localPos;
        gameObject.SetActive(true);
        UpdateTimerText();

        rectTransform.DOLocalMoveX(0f, animSettings.genericTweenDuration)
            .SetDelay(animSettings.interScreenDelay)
            .SetEase(animSettings.genericEasing);
        
        canvasGroup.DOFade(1f, animSettings.genericTweenDuration)
            .SetDelay(animSettings.interScreenDelay)
            .SetEase(animSettings.genericEasing)
            .OnComplete(() => { canvasGroup.blocksRaycasts = true; });
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;

        rectTransform.DOLocalMoveX(animSettings.timerButtonPositionOffset, animSettings.genericTweenDuration)
            .SetEase(animSettings.genericEasing);
        
        canvasGroup.DOFade(0f, animSettings.genericTweenDuration)
            .SetEase(animSettings.genericEasing)
            .OnComplete(() => { gameObject.SetActive(false); });
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