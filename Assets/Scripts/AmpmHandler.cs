using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpmHandler : MonoBehaviour
{
    public GameObject circle;
    private ClockHandler clockHandler;

    public void Start()
    {
        clockHandler = GameObject.FindWithTag("clock").GetComponent<ClockHandler>();
    }
    public void OnClick()
    {
        clockHandler.isAMPM = !clockHandler.isAMPM;
        moveCircle();

        clockHandler.sendData("set_colon_mode", clockHandler.isAMPM ? "1" : "0");
    }

    public void moveCircle()
    {
        StartCoroutine(SmoothMove());
    }
    IEnumerator SmoothMove()
    {
        Vector3 startpos = circle.transform.localPosition, endpos;
        endpos = clockHandler.isAMPM ? new Vector3(36, 0, 0) : new Vector3(-36, 0, 0);
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
