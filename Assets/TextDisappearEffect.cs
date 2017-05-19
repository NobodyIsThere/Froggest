using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisappearEffect : MonoBehaviour
{
    public float FadeTime = 0.3f;
    public float start_size = 0.1f;
    public float end_size = 0.4f;

    private TextMesh mesh;
    private new MeshRenderer renderer;
    private float start_time;
    private float r;
    private float g;
    private float b;

    // Use this for initialization
    void Start ()
    {
        start_time = Time.time;
        mesh = GetComponent<TextMesh>();
        start_size = mesh.characterSize;
        renderer = GetComponent<MeshRenderer>();
        r = renderer.material.color.r;
        g = renderer.material.color.g;
        b = renderer.material.color.b;
    }
    
    // Update is called once per frame
    void Update ()
    {
        float t = (Time.time - start_time)/FadeTime;
        if (t > 1f)
        {
            Destroy(gameObject);
        }
        mesh.characterSize = Mathf.Lerp(start_size, end_size, t);
        renderer.material.color = new Color(r, g, b, Mathf.Lerp(1f, 0f, t));
    }
}
