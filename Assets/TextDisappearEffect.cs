using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisappearEffect : MonoBehaviour
{
    public float FadeTime = 0.3f;
    private float start_size = 1f;
    public float end_size = 1.5f;

    private RectTransform[] rects;
    private Text[] texts;
    private float start_time;

    // Use this for initialization
    void Start ()
    {
        start_time = Time.time;
        RectTransform[] all_rects = GetComponentsInChildren<RectTransform>();
        if (GetComponent<RectTransform>() != null)
        {
            rects = new RectTransform[all_rects.Length - 1];
            int i = 0;
            foreach (RectTransform rect in all_rects)
            {
                if (rect.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                {
                    rects[i++] = rect;
                }
            }
        }
        else
        {
            rects = all_rects;
        }
        start_size = rects[0].localScale.x;
        end_size = start_size * end_size;
        texts = GetComponentsInChildren<Text>();
    }
    
    // Update is called once per frame
    void Update ()
    {
        float t = (Time.time - start_time)/FadeTime;
        if (t > 1f)
        {
            Destroy(gameObject);
        }
        float scale = Mathf.Lerp(start_size, end_size, t);
        foreach (RectTransform rect in rects)
        {
            if (rect.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                rect.localScale = new Vector3(scale, scale, scale);
        }
        foreach (Text text in texts)
        {
            if (text.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, t));
        }
    }
}
