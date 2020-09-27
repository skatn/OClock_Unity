using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockHandler : MonoBehaviour
{
    public GameObject[] timeDisplay;
    public int time = 0, minute = 0, timeOffset = 0;
    public int brightness = 100;
    public bool isHour24 = false, isAutoBright = false, isAMPM = false;
    private TimeHandler timeHandler;
    private BrightHandler brightHandler;
    private AmpmHandler ampmHandler;
    private BrightBarHandler brightBarHandler;

    public void Start()
    {
        timeHandler = GameObject.FindWithTag("time").GetComponent<TimeHandler>();
        brightHandler = GameObject.FindWithTag("auto bright").GetComponent<BrightHandler>();
        ampmHandler = GameObject.FindWithTag("show ampm").GetComponent<AmpmHandler>();
    }

    public void OnClickSync()
    {
        StartCoroutine(WaitForSync());
    }

    public void ShowTime()
    {
        int buf = time;
        if (buf > 12) buf -= 12;
        else if (buf == 0) buf = 12;

        timeDisplay[0].GetComponent<Text>().text = ((isHour24 ? time : buf) / 10).ToString();
        timeDisplay[1].GetComponent<Text>().text = ((isHour24 ? time : buf) % 10).ToString();

        timeDisplay[3].GetComponent<Text>().text = (minute / 10).ToString();
        timeDisplay[4].GetComponent<Text>().text = (minute % 10).ToString();

        Debug.Log("time offset : " + timeOffset.ToString());
    }

    public void setAlpha(float value)
    {
        for(int i=0; i<5; i++)
        {
            timeDisplay[i].GetComponent<Text>().color = new Color(1, 1, 1, value);
        }
    }

    public void sendData(string key, string value)
    {
        StartCoroutine(WaitForRequest(key, value));
    }

    IEnumerator WaitForRequest(string key, string value)
    {
        WWWForm form = new WWWForm();
        form.AddField(key, value);

        WWW www = new WWW("http://192.168.4.1/post", form);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW OK: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    IEnumerator WaitForSync()
    {
        WWWForm form = new WWWForm();
        form.AddField("sync_request", "");

        WWW www = new WWW("http://192.168.4.1/post", form);
        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWW OK: " + www.text);
            string[] split = www.text.Split(',');

            time = int.Parse(split[0]) / 100;
            minute = int.Parse(split[0]) % 100;
            timeOffset = int.Parse(split[1]);

            isHour24 = int.Parse(split[2]) != 1;
            isAMPM = int.Parse(split[3]) == 1;

            isAutoBright = int.Parse(split[4]) == 1;

            brightness = int.Parse(split[5]);


            ShowTime();

            timeHandler.ShowTime();
            timeHandler.moveCircle();

            ampmHandler.moveCircle();
            brightHandler.moveCircle();
            brightBarHandler.updateBar();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
