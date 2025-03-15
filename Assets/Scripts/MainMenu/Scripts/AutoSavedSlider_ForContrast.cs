//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.HighDefinition;

public class AutoSavedSlider_ForContrast : AutoSavedSlider
{
    [SerializeField] Volume globalVolume;
    private UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
    private float minValue = -40;
    private float maxValue = 40;


    public override void InternalValueChanged(float value)
    {
        float interpolatedValue = Mathf.Lerp(minValue, maxValue, value);

        globalVolume.profile.TryGet(out colorAdjustments);
        colorAdjustments.contrast.value = interpolatedValue;
    }
}