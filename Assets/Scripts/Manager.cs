using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Canvas rootCanvas;
    public RectTransform buttonGroupParent;
    public TimerButton timerButtonPrefab;

    private List<TimerButton> _timers;

    private void Start()
    {
        const int numOfTimers = 4;
        const float margin = 8f;

        _timers = new List<TimerButton>(numOfTimers);
        var canvasTransform = rootCanvas.transform;
        var timerHeight = timerButtonPrefab.rectTransform.rect.height;
        int floorHalf = numOfTimers / 2;
        var initialOffset = floorHalf * (timerHeight + floorHalf * margin);
        for (int i = 0; i < numOfTimers; i++)
        {
            var timer = Instantiate(timerButtonPrefab, canvasTransform);
            timer.transform.localPosition = new Vector3(0f, -i * timerHeight - margin * i + initialOffset, 0f);
            timer.Init(this, i);
            _timers.Add(timer);
        }
    }

    public void OnTimerClicked(int index)
    {
    }
}
