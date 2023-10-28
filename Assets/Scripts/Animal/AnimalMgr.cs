using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责动物的生成
/// </summary>
public class AnimalMgr : MonoBehaviour
{
    [Header("动物生成的间隔时间")]
    public float animalSpawnTime = 16f;
    [Header("动物生成的间隔时间上下浮动值")]
    public float animalSpawnTimeBias = 4f;

    public Dictionary<string, AnimalSetting> settings = new Dictionary<string, AnimalSetting>();

    private void Start()
    {
        foreach (var setting in FindObjectsOfType<AnimalSetting>())
        {
            settings[setting.animalName] = setting;
        }
        GameObject.Find("Cat").GetComponent<Animal>().Init();
    }
}
