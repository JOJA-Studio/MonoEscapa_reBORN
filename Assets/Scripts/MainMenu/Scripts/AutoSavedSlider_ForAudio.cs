using UnityEngine;
using UnityEngine.Audio;

public class AutoSavedSlider_ForAudio : AutoSavedSlider
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string nameParametro;


    public override void InternalValueChanged(float value)
    {
        audioMixer.SetFloat(nameParametro, LinearToDecibel(value));
    }

    private float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;
        return dB;
    }
}
