using UnityEngine;
using UnityEngine.UI;
public class AutoSavedSlider : MonoBehaviour
{
    [SerializeField] string savedSlider;
    [SerializeField] float initialValue;

    private Slider slider;

    public virtual void InternalValueChanged(float value)
    {

    }

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        slider.value = PlayerPrefs.GetFloat(savedSlider, initialValue);

        //Cambiar el Slider value por el valor recuperado de playerprefs: savedSlider
    }

    void Start()
    {
        InternalValueChanged(slider.value);
    }

    private void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(savedSlider, value);
        PlayerPrefs.Save();

        InternalValueChanged(value);
    }
}
