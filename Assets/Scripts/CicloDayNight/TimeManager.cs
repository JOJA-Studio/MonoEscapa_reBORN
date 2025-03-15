using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]private Texture2D skyboxNight;
    [SerializeField]private Texture2D skyboxSunRise;
    [SerializeField]private Texture2D skyboxDay;
    [SerializeField]private Texture2D skyboxSunset;

    [SerializeField]private Light globalLight;

    [SerializeField]private Gradient NightToSunrise;
    [SerializeField]private Gradient SunriseToDay;
    [SerializeField]private Gradient DayToSunset;
    [SerializeField]private Gradient SunsetToNight;
    [SerializeField] public int minutos;
    public int Min { get { return minutos; }set { minutos = value;OnMinuteChange(value); } }
    private int horas;
    public int H { get { return horas; } set { horas = value; OnHourChange(value); } }
    private int dias;
    public int D { get { return dias; } set { dias = value; } }
    private float tempSeconds;
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 2f;
    }

    // Update is called once per frame
    public void Update()
    {
        tempSeconds += Time.deltaTime;
        if (tempSeconds >= 1)
        {
            Min += 1;
            tempSeconds = 0;
        }
    }
    private void OnMinuteChange(int value)
    {
        globalLight.transform.Rotate(Vector3.up, (1f / (1440f / 4f)) * 360f, Space.World);
        globalLight.transform.Rotate(Vector3.right, (1f / (1440f / 4f)) * 360f, Space.World);
        if (value >= 60) 
        {
            horas++;
            minutos = 0;
        }
        if (horas >= 24)
        {
            horas = 0;
            dias++;
        }
    }
    private void OnHourChange(int value)
    {
        if (value == 6)
        {
            StartCoroutine(LerpSkybox(skyboxNight,skyboxSunRise, 10f));
            StartCoroutine(LerpLight(NightToSunrise, 10f));
        }
        else if (value == 8) 
        {
            StartCoroutine(LerpSkybox(skyboxSunRise, skyboxDay, 10f));
            StartCoroutine(LerpLight(SunriseToDay, 10f));
        }
        else if (value == 18) 
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 10f));
            StartCoroutine(LerpLight(DayToSunset, 10f));
        }
        else if (value == 22) 
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 10f));
            StartCoroutine(LerpLight(SunsetToNight, 10f));
        }
    }
    private IEnumerator LerpSkybox(Texture2D a,Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1",a);
        RenderSettings.skybox.SetTexture("_Texture2",b);
        RenderSettings.skybox.SetFloat("_Blend",0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend",i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }
    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}
