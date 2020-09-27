using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebView : MonoBehaviour
{
    public void OnClick()
    {
        Application.OpenURL("http://192.168.4.1/wifi?");
    }
}
