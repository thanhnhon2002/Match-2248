using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

public class TimeCounter : MonoBehaviour
{
    public enum TimeShowType
    {
        Seconds, Minutes, Hours, Minutes_Seconds, Minutes_Hours, Full
    }

    public double WAIT_TIME;
    public TimeShowType type;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private string message;
    public UnityEvent timeUp;
    [SerializeField] private bool hasInvoked;
    public double second;

    private void OnEnable()
    {
        SetTime();
    }

    void Update()
    {
        if (second <= 0)
        {
            second = 0f;
            if(!hasInvoked)
            {
                hasInvoked = true;
                timeUp?.Invoke();             
            }
        }
        else second -= Time.deltaTime;
        var timeSpan = TimeSpan.FromSeconds(second);
        var timeTxt = FormatTime(timeSpan);
        timer.text = timeTxt;
    }

    public void ResetTimer()
    {
        second = WAIT_TIME;
        hasInvoked = false;
    }
    public void SetTime()
    {
        second = WAIT_TIME;
        var timeSpan = TimeSpan.FromSeconds(second);
        timer.text = FormatTime(timeSpan);
    }

    public void SetTime(double time)
    {
        WAIT_TIME = time;
        SetTime();
    }

    private string FormatTime(TimeSpan timeSpan)
    {
        return type switch
        {
            TimeShowType.Seconds => message + string.Format("{0:D2}", Mathf.FloorToInt((float)timeSpan.Seconds % 60)),
            TimeShowType.Minutes => message + string.Format("{0:D2}", timeSpan.Minutes),
            TimeShowType.Hours => message + string.Format("{0:D2}", timeSpan.Hours),
            TimeShowType.Minutes_Seconds => message + string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, Mathf.FloorToInt((float)timeSpan.Seconds % 60)),
            TimeShowType.Minutes_Hours => message + string.Format("{0:D2}:{1:D2}", timeSpan.Hours, timeSpan.Minutes),
            _=> message + string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, Mathf.FloorToInt((float)timeSpan.Seconds % 60))
        };
    }
}
