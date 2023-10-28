using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����������
/// </summary>
public class AnimalMgr : MonoBehaviour
{
    [Header("�������ɵļ��ʱ��")]
    public float animalSpawnTime = 16f;
    [Header("�������ɵļ��ʱ�����¸���ֵ")]
    public float animalSpawnTimeBias = 4f;

    public Dictionary<string, AnimalSetting> settings = new Dictionary<string, AnimalSetting>();

    private void Start()
    {
        foreach (var setting in FindObjectsOfType<AnimalSetting>())
        {
            settings[setting.animalName] = setting;
        }
        foreach (var animal in FindObjectsOfType<Animal>())
        {
            animal.Init();
        }
    }
}
