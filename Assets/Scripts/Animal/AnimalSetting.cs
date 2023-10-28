using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSetting : MonoBehaviour
{
    [Header("��������")]
    public string animalName;

    [Space(40)]
    [Header("������ſ���ʱ��")]
    public float lieMinTime;
    [Header("������ſ�ʱ��")]
    public float lieMaxTime;

    [Space(40)]
    [Header("����վ�����ʱ��")]
    public float standMinTime;
    [Header("����վ���ʱ��")]
    public float standMaxTime;

    [Space(40)]
    [Header("�����������ʱ��")]
    public float walkMinTime;
    [Header("���������ʱ��")]
    public float walkMaxTime;
    [Header("������������")]
    public float walkSpeed;

    [Space(40)]
    [Header("����������ʱ��")]
    public float runMinTime;
    [Header("��������ʱ��")]
    public float runMaxTime;
    [Header("�����������")]
    public float runSpeed;

    [Space(40)]
    [Header("���������̾���")]
    public float runMinDist;
    [Header("������������")]
    public float runMaxDist;

    [Space(40)]
    [Header("����ת���ж�ʱ��")]
    public float turnCheckTime;
    [Header("����ת�����")]
    public float trunCheckProb;
    [Header("���������ƶ����پ������ת���ж�")]
    public float trunCheckDist;

    [Space(40)]
    [Header("����ε�ˮʱ�õ��ķ���")]
    public int waterEnterScore;
    [Header("����ˮ����ʱ�õ��ķ���")]
    public int waterFullScore;
    [Header("���������Ҫ��ˮ����")]
    public int waterCapa;
    [Header("���������Ҫ��ˮ�������¸���ֵ")]
    public int waterCapaBias;

    [Space(40)]
    [Header("����Ȩ��")]
    public int spawnWeight;
}
