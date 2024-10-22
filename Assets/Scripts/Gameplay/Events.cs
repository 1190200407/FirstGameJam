using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelLoadEvent
{
}

public struct StateChangeEvent
{
    public PlayerState newState;
}

public struct DestroyObjectEvent
{
    public GameObject gameObject;
}

public struct SelectItemEvent
{
    public GameItemData itemData;
}

public struct UseItemEvent
{
    public GameItemData itemData;
    public int newCount;
}

public struct CannotUseItemEvent
{
    public GameItemData itemData;
}

public struct GetItemEvent
{
    public GameItemData itemData;
    public int newCount;
}

public struct PutoutCandleEvent
{
    public Candle candle;
}

public struct LightCandleEvent
{
    public Candle candle;
}

public struct TouchFireflyEvent
{
    public ColorFirefly colorFirefly;
}

public struct PlaySoundEvent
{
    public string soundName;
    public float volumeScale;
}