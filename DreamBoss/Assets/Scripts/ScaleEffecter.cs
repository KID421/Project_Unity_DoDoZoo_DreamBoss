using System;
using UnityEngine;

public class ScaleEffecter : MonoBehaviour
{
    [Header("縮放幅度"), Range(0f, 10f)]
    public float scale = 0.2f;
    [Header("縮放速度"), Range(0f, 10f)]
    public float speed = 2f;

    private float scaleOriginal;

    private void Awake()
    {
        scaleOriginal = transform.localScale.x;
    }

    private void Update()
    {
        StartScaleEffecter();
    }

    private void StartScaleEffecter()
    {
        transform.localScale = Vector3.one * (scaleOriginal + (float)Math.Sin(Time.time * speed) * scale);
    }
}
