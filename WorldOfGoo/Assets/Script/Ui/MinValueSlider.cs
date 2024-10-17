using UnityEngine;
using UnityEngine.UI;

public class MinValueSlider : MonoBehaviour
{
    private Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (slider.value == -30)
        {
            slider.minValue = -80;
            slider.value = slider.minValue;
        }
        else if (slider.minValue == -80 && slider.value != -80)
        {
            slider.minValue = -30;
            slider.value = slider.minValue + 1;
        }
    }
}