using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Totem : Interactable
{
    public Transform pivotPoint;
    public List<Candle> candles;
    public List<FanshapedLight> lights;
    private int lightLevel = 0;
    public bool IsFacingRight { get; private set; }

    void Start()
    {
        lightLevel = 0;
        foreach (var candle in candles)
        {
            if (candle.litAtStart)
            {
                AddLightLevel();
            }
        }
    }

    void OnEnable()
    {
        MyEventSystem.Register<LightCandleEvent>(OnLightCandle);
        MyEventSystem.Register<PutoutCandleEvent>(OnPutoutCandle);
    }

    void OnDisable()
    {
        MyEventSystem.Unregister<LightCandleEvent>(OnLightCandle);
        MyEventSystem.Unregister<PutoutCandleEvent>(OnPutoutCandle);
    }

    public void OnLightCandle(LightCandleEvent @event)
    {
        if (candles.Contains(@event.candle))
        {
            AddLightLevel();
        }
    }

    public void OnPutoutCandle(PutoutCandleEvent @event)
    {
        if (candles.Contains(@event.candle))
        {
            ReduceLightLevel();
        }
    }

    public void AddLightLevel()
    {
        if (lightLevel < lights.Count)
        {
            if (lightLevel != 0)
                lights[lightLevel - 1].enabled = false;
            lightLevel++;
            lights[lightLevel - 1].enabled = true;
        }
    }

    public void ReduceLightLevel()
    {
        if (lightLevel > 0)
        {
            lights[lightLevel - 1].enabled = false;
            lightLevel--;
            if (lightLevel > 0)
                lights[lightLevel - 1].enabled = true;
        }
    }

    public override void Interact()
    {
        Turn();
        AudioManager.Instance.PlaySfx("通用交互");
    }

    public void Turn()
    {
        IsFacingRight = !IsFacingRight;
        foreach (var light in lights)
        {
            light.startPoint.position = MyMathUtil.ReversePoint(light.startPoint.position, pivotPoint.position);
            light.cutPoint1.position = MyMathUtil.ReversePoint(light.cutPoint1.position, pivotPoint.position);
            light.cutPoint2.position = MyMathUtil.ReversePoint(light.cutPoint2.position, pivotPoint.position);
        }
    }
}
