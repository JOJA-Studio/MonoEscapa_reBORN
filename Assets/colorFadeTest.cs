using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorFadeTest : MonoBehaviour
{
    public Material material;
    public Color startColor = Color.red;
    public Color endColor = Color.gray;
    public float duration = 5f;
    private bool isFading = false; // <-- Flag para controlar si ya está en proceso

    private void Start()
    {
        material.color = startColor;
    }

    private void FixedUpdate()
    {
        if (Input.anyKeyDown && !isFading) // <-- Solo entra si aún no está en fade
        {
            StartColorFade();
        }
    }

    public void StartColorFade()
    {
        isFading = true; // <-- Marcamos que se ha iniciado
        StartCoroutine(FadeColor());
    }

    private IEnumerator FadeColor()
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            material.color = Color.Lerp(startColor, endColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        material.color = endColor;
    }
}
