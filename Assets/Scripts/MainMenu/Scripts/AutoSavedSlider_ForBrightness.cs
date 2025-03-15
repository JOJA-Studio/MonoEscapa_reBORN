//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.HighDefinition;

public class AutoSavedSlider_ForBrigthness : AutoSavedSlider
{
    [SerializeField] Volume globalVolume;
    private UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
    private float minValue = -2;
    private float maxValue = 2;


    public override void InternalValueChanged(float value)
    {
        float interpolatedValue = Mathf.Lerp(minValue, maxValue, value);

        globalVolume.profile.TryGet(out colorAdjustments);
        colorAdjustments.postExposure.value = interpolatedValue;
    }
}