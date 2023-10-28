using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider energySlider;
    public float dePerSec = 10.0f;//decrease amount every time delta;
    public bool isEmpty {  get; private set; }

    void ChangeEnergy(float amount)
    {
        energySlider.value = Mathf.Clamp(energySlider.value - amount, 0, energySlider.maxValue);
    }

    void Start()
    {
        energySlider = GetComponent<Slider>();
        energySlider.value = energySlider.maxValue;
        isEmpty = false;
    }

    void Update()
    {
        if (GameCtr.instance.waterCtr.IsOpen)
        {
            //UnityEngine.Debug.Log("pot is open");
            ChangeEnergy(dePerSec * Time.deltaTime);
        }
        if (energySlider.value <= 0.05f && !isEmpty)
        {
            UnityEngine.Debug.Log("should close");
            isEmpty = true;
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
