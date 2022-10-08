using TMPro;
using UnityEngine;

public class TimerButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform rectTransform;

    private Manager _manager;
    private int _index;

    public void Init(Manager manager, int index)
    {
        _manager = manager;
        _index = index;
        text.text = $"Timer {index + 1}";
    }

    public void OnClick()
    {
        _manager.OnTimerClicked(_index);
    }
}
