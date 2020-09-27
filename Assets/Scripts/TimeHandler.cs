using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHandler : MonoBehaviour
{
    public GameObject[] timeControl;
    public GameObject circle;
    private ClockHandler clockHandler;

    public void Start()
    {
        clockHandler = GameObject.FindWithTag("clock").GetComponent<ClockHandler>();
    }

    public void TimeUp()
    {
        ChangeTime(1);
        ShowTime();
    }
    public void TimeDown()
    {
        ChangeTime(-1);
        ShowTime();
    }

    public void MinuteUp()
    {
        ChangeMinute(1);
        ShowTime();
    }
    public void MinuteDown()
    {
        ChangeMinute(-1);
        ShowTime();
    }
    public void MinuteUp10()
    {
        ChangeMinute(10);
        ShowTime();
    }

    public void MinuteDown10()
    {
        ChangeMinute(-10);
        ShowTime();
    }

    public void SetAM()
    {
        if (timeControl[0].GetComponent<Text>().text.Equals("오후"))
        {
            ChangeTime(-12);
            ShowTime();
        }
    }
    public void SetPM()
    {
        if (timeControl[0].GetComponent<Text>().text.Equals("오전"))
        {
            ChangeTime(12);
            ShowTime();
        }
    }

    private void ChangeTime(int change)
    {
        int time = clockHandler.time;
        time += change;
        if (time > 23) time = 0;
        else if (time < 0) time = 23;

        clockHandler.timeOffset += (time - clockHandler.time) * 60 * 60;
        clockHandler.time = time;

        clockHandler.sendData("set_time", clockHandler.timeOffset.ToString());
    }

    private void ChangeMinute(int change)
    {
        int minute = clockHandler.minute;
        minute += change;
        if(minute > 59)
        {
            ChangeTime(1);
            minute -= 60;
        }
        else if(minute < 0)
        {
            ChangeTime(-1);
            minute += 60;
        }
        clockHandler.timeOffset += (minute - clockHandler.minute) * 60;
        clockHandler.minute = minute;

        clockHandler.sendData("set_time", clockHandler.timeOffset.ToString());
    }

    public void ToggleMode()
    {
        clockHandler.isHour24 = !clockHandler.isHour24;
        ShowTime();
        moveCircle();

        clockHandler.sendData("set_hour_mode", clockHandler.isHour24 ? "0" : "1");
    }

    public void ShowTime()
    {
        int time = clockHandler.time;
        int minute = clockHandler.minute;

        int buf = time;
        if (buf > 12) buf -= 12;
        else if (buf == 0) buf = 12;

        if(time >= 12)
        {
            timeControl[0].GetComponent<Text>().text = "오후";
        }
        else
        {
            timeControl[0].GetComponent<Text>().text = "오전";
        }
        timeControl[1].GetComponent<Text>().text = buf.ToString();
        timeControl[2].GetComponent<Text>().text = (minute / 10).ToString();
        timeControl[3].GetComponent<Text>().text = (minute % 10).ToString();

        clockHandler.ShowTime();
    }

    public void moveCircle()
    {
        StartCoroutine(SmoothMove());
    }

    IEnumerator SmoothMove()
    {
        Vector3 startpos = circle.transform.localPosition, endpos;
        endpos = clockHandler.isHour24 ? new Vector3(36, 0, 0) : new Vector3(-36, 0, 0);
        float t = 0f, seconds = 0.2f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            circle.transform.localPosition = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        circle.transform.localPosition = endpos;
    }
}
