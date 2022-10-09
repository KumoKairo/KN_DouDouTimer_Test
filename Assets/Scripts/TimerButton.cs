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
    public CanvasGroup canvasGroup;

    private int _index;
    private Manager _manager;

    private Sequence _currentlyRunningSequence;

    public void Init(Manager manager, int index)
    {
        _manager = manager;
        _index = index;
        text.text = $"Timer {index + 1}";

        canvasGroup.blocksRaycasts = false;
        var localPos = rectTransform.localPosition;
        localPos.x = -manager.rootCanvas.GetComponent<CanvasScaler>().referenceResolution.x * 0.5f -
                     rectTransform.rect.width;

        rectTransform.localPosition = localPos;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);

        var anim = DOTween.Sequence();
        anim.Insert(0f, rectTransform.DOLocalMoveX(0f, animSettings.genericTweenDuration, true));
        anim.Insert(0f, canvasGroup.DOFade(1f, animSettings.genericTweenDuration));
        anim.SetDelay(animSettings.genericDelay * index);
        anim.OnComplete(() => { canvasGroup.blocksRaycasts = true; });
    }

    public void OnClick()
    {
        _manager.OnTimerClicked(_index);
    }

    public void DisableAndHide()
    {
        canvasGroup.blocksRaycasts = false;

        if (_currentlyRunningSequence != null)
        {
            _currentlyRunningSequence.Complete(true);
        }
        
        var anim = DOTween.Sequence();
        anim.Insert(0f, rectTransform.DOLocalMoveX(-animSettings.timerButtonPositionOffset,
            animSettings.genericTweenDuration,
            true));
        anim.Insert(0f, canvasGroup.DOFade(0f, animSettings.genericTweenDuration));
        anim.SetEase(animSettings.genericEasing);
        anim.SetDelay(animSettings.genericDelay * _index);
        anim.OnComplete(() => { 
            gameObject.SetActive(false);
            _currentlyRunningSequence = null;
        });

        _currentlyRunningSequence = anim;
    }

    public void EnableAndShow()
    {
        if (_currentlyRunningSequence != null)
        {
            _currentlyRunningSequence.Complete(true);
        }
        
        gameObject.SetActive(true);

        var anim = DOTween.Sequence();
        anim.Insert(0f,
            rectTransform.DOLocalMoveX(0f, animSettings.genericTweenDuration, true));

        anim.Insert(0f, canvasGroup.DOFade(1f, animSettings.genericTweenDuration));
        anim.SetEase(animSettings.genericEasing);
        anim.OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = true;
            _currentlyRunningSequence = null;
        });
        
        _currentlyRunningSequence = anim;
    }
}