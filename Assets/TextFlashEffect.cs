using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlashEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // First clone our parent
        RectTransform rect = GetComponent<RectTransform>();
        Text text = GetComponent<Text>();

        RectTransform parent_rect = transform.parent.GetComponent<RectTransform>();
        Text parent_text = transform.parent.GetComponent<Text>();

        // Now make a canvas and make that our parent.
        GameObject g = new GameObject("Flash");
        g.transform.parent = parent_rect.parent;
        g.transform.localPosition = Vector3.zero;
        transform.SetParent(g.transform);

        rect.pivot = parent_rect.pivot;
        rect.anchoredPosition = parent_rect.anchoredPosition;
        rect.sizeDelta = parent_rect.sizeDelta;
        rect.localScale = parent_rect.localScale;

        text.font = parent_text.font;
        text.fontStyle = parent_text.fontStyle;
        text.fontSize = parent_text.fontSize;
        text.alignment = parent_text.alignment;

        text.color = parent_text.color;

        text.text = parent_text.text;

        // And disappear?
        g.AddComponent<TextDisappearEffect>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
