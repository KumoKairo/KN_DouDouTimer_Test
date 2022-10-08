using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerButton : MonoBehaviour
{
    public AnimationSettings animSettings;
    
    [Space] public TextMeshProUGUI text;
    public RectTransform rectTransform;
    public Button button;
    public CanvasGroup canvasGroup;
    
    private int _index;
    private Manager _manager;

    public void Init(Manager manager, int index)
    {
        _manager = manager;
        _index = index;
        text.text = $"Timer {index + 1}";

        button.enabled = false;
        var localPos = rectTransform.localPosition;
        var initialX = localPos.x;
        localPos.x += animSettings.timerButtonPositionOffset;
        rectTransform.localPosition = localPos;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);

        var anim = DOTween.Sequence();
        anim.Insert(0f, rectTransform.DOLocalMoveX(initialX, animSettings.genericTweenDuration, true));
        anim.Insert(0f, canvasGroup.DOFade(1f, animSettings.genericTweenDuration));
        anim.SetDelay(animSettings.genericDelay * index);
        anim.OnComplete(() =>
        {
            button.enabled = true;
        });
    }

    public void OnClick()
    {
        _manager.OnTimerClicked(_index);
    }

    public void DisableAndHide()
    {
        button.interactable = false;
        gameObject.SetActive(false);
    }

    public void EnableAndShow()
    {
        gameObject.SetActive(true);
        button.interactable = true;
    }
}