using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrightBarHandler : MonoBehaviour, IPointerUpHandler
{
    public Slider brightBar;
    private ClockHandler clockHandler;
    // Start is called before the first frame update
    void Start()
    {
        clockHandler = GameObject.FindWithTag("clock").GetComponent<ClockHandler>();
        updateBar();
    }

    // Update is called once per frame
    void Update()
    {
        clockHandler.setAlpha(brightBar.value);
    }

    public void updateBar()
    {
        brightBar.value = clockHandler.brightness / 100f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clockHandler.brightness = (int)(brightBar.value * 100);
        clockHandler.sendData("set_brightness", clockHandler.brightness.ToString());
    }
}
