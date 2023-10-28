using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class EngergyBar : MonoBehaviour
{
    public WaterCtr waterCtr;
    public Slider energySlider;
    public float dePerSec = 10.0f;//decrease amount every time delta;

    void ChangeEnergy(float amount)
    {
        energySlider.value = Mathf.Clamp(energySlider.value - amount, 0, energySlider.maxValue);
    }

    void Start()
    {
        energySlider.value = energySlider.maxValue;
    }

    void Update()
    {
        if (waterCtr.IsOpen)
        {
            UnityEngine.Debug.Log("pot is open");
            ChangeEnergy(dePerSec * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.Debug.Log("space key was pressed");
            ChangeEnergy(dePerSec * Time.deltaTime);
        }
    }

    public void SetDecreaseAmount(float amount)
    {
        dePerSec = amount;
    }

    public void SetMaxEnergy(float newMaxEnergy)
    {

        energySlider.maxValue = newMaxEnergy;
    }
}
