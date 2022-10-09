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

    private Sequence _currentlyRunningInOutSequence;
    private Sequence _wobbleLoopSequence;

    public void Init(Manager manager, int index)
    {
        _manager = manager;
        _index = index;
        text.text = $"Timer {index + 1}";
    }

    public void TweenInFromSide(Manager manager)
    {
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
        anim.SetDelay(animSettings.genericDelay * _index);

        anim.OnComplete(() => { canvasGroup.blocksRaycasts = true; });
    }

    public void TweenInFromBottom()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);

        var anim = DOTween.Sequence();
        anim.Insert(0f, canvasGroup.DOFade(1f, animSettings.genericTweenDuration));
        anim.OnComplete(() => { canvasGroup.blocksRaycasts = true; });
    }

    public void OnClick()
    {
        if (_wobbleLoopSequence != null)
        {
            _wobbleLoopSequence.Kill();
            rectTransform.localScale = Vector3.one;
            rectTransform.localRotation = Quaternion.identity;
            _wobbleLoopSequence = null;
        }

        _manager.OnTimerClicked(_index);
    }

    public void DisableAndHide()
    {
        canvasGroup.blocksRaycasts = false;

        if (_currentlyRunningInOutSequence != null)
        {
            _currentlyRunningInOutSequence.Complete(true);
        }

        var anim = DOTween.Sequence();
        anim.Insert(0f, rectTransform.DOLocalMoveX(-animSettings.timerButtonPositionOffset,
            animSettings.genericTweenDuration,
            true));
        anim.Insert(0f, canvasGroup.DOFade(0f, animSettings.genericTweenDuration));
        anim.SetEase(animSettings.genericEasing);
        anim.SetDelay(animSettings.genericDelay * _index);
        anim.OnComplete(() =>
        {
            gameObject.SetActive(false);
            _currentlyRunningInOutSequence = null;
        });

        _currentlyRunningInOutSequence = anim;
    }

    public void EnableAndShow()
    {
        if (_currentlyRunningInOutSequence != null)
        {
            _currentlyRunningInOutSequence.Complete(true);
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
            _currentlyRunningInOutSequence = null;
        });

        _currentlyRunningInOutSequence = anim;
    }

    public void EaseOutDestroy()
    {
        var newY = rectTransform.localPosition.y - rectTransform.rect.height;
        canvasGroup.DOFade(0f, animSettings.genericTweenDuration);
        rectTransform.DOLocalMoveY(newY, animSettings.genericTweenDuration)
            .SetEase(animSettings.genericEasing)
            .OnComplete(() => { Destroy(gameObject); });
    }

    public void PlayCompletedAnimation()
    {
        var anim = DOTween.Sequence();
        anim.Insert(0f, rectTransform.DOPunchScale(Vector3.one * animSettings.timerWobbleStrength,
            animSettings.timerWobbleDuration,
            animSettings.timerWobbleVibrato, animSettings.timerWobbleElasticity));
        anim.SetLoops(-1, LoopType.Restart);
        _wobbleLoopSequence = anim;
    }
}