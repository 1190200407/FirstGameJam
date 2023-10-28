using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSetting : MonoBehaviour
{
    [Header("动物名字")]
    public string animalName;

    [Space(40)]
    [Header("动物窝趴最短时间")]
    public float lieMinTime;
    [Header("动物窝趴最长时间")]
    public float lieMaxTime;

    [Space(40)]
    [Header("动物站立最短时间")]
    public float standMinTime;
    [Header("动物站立最长时间")]
    public float standMaxTime;

    [Space(40)]
    [Header("动物慢走最短时间")]
    public float walkMinTime;
    [Header("动物慢走最长时间")]
    public float walkMaxTime;
    [Header("动物慢走移速")]
    public float walkSpeed;

    [Space(40)]
    [Header("动物快跑最短时间")]
    public float runMinTime;
    [Header("动物快跑最长时间")]
    public float runMaxTime;
    [Header("动物快跑移速")]
    public float runSpeed;

    [Space(40)]
    [Header("动物飞跳最短距离")]
    public float runMinDist;
    [Header("动物飞跳最长距离")]
    public float runMaxDist;

    [Space(40)]
    [Header("动物转向判定时间")]
    public float turnCheckTime;
    [Header("动物转向概率")]
    public float trunCheckProb;
    [Header("动物至少移动多少距离才能转向判定")]
    public float trunCheckDist;

    [Space(40)]
    [Header("动物滴到水时得到的分数")]
    public int waterEnterScore;
    [Header("动物水滴满时得到的分数")]
    public int waterFullScore;
    [Header("动物滴满需要的水滴数")]
    public int waterCapa;
    [Header("动物滴满需要的水滴数上下浮动值")]
    public int waterCapaBias;

    [Space(40)]
    [Header("生成权重")]
    public int spawnWeight;
}
