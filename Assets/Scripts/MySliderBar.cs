using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySliderBar : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 0f;
    }

    public void IncreaseTo(float amount)
    {
        slider.value = Mathf.Clamp(amount, 0, slider.maxValue);
    }
    public void Increase(float amount)
    {
        slider.value = Mathf.Clamp(slider.value + amount, 0, slider.maxValue);
    }
    public float Value() { return slider.value; }
}
