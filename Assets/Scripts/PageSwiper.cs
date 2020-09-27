using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    private int step = 0;
    public GameObject[] indicator;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if(percentage > 0 && step < indicator.Length-1)
            {
                Fadeout(step);
                newLocation += new Vector3(-Screen.width, 0, 0);
                step++;
            }
            else if(percentage < 0 && step > 0)
            {
                Fadeout(step);
                newLocation += new Vector3(Screen.width, 0, 0);
                step--;
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
            Debug.Log(step);
            Fadein(step);
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    private void Fadein(int step)
    {
        indicator[step].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 1f);
        StartCoroutine(ChangeIndicator(indicator[step], 1f, easing));
    }

    private void Fadeout(int step)
    {
        indicator[step].GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 100/255f);
        StartCoroutine(ChangeIndicator(indicator[step], 0.7f, easing));
    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while(t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    IEnumerator ChangeIndicator(GameObject obj, float toScale, float seconds)
    {
        float t = 0f, size = 0, startScale = obj.transform.localScale.x;
        while (t <= seconds)
        {
            t += Time.deltaTime;
            size = (toScale - startScale) * (t / seconds) + startScale;
            obj.transform.localScale = new Vector3(size, size, size);
            yield return null;
        }
    }
}
