using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerButton : MonoBehaviour
{
    [Space] public TextMeshProUGUI text;
    public RectTransform rectTransform;
    public Button button;
    
    private int _index;
    private Manager _manager;

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