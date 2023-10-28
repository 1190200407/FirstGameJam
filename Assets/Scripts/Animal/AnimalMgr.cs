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

    public List<SpawnPlace> spawnPlaces;
    public List<GameObject> animalPrefabs;
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
        StartCoroutine(SpawnAnimal());
    }

    private IEnumerator SpawnAnimal()
    {
        while (true)
        {
            float time = Random.Range(-animalSpawnTimeBias, animalSpawnTimeBias) + animalSpawnTime;
            yield return new WaitForSeconds(time);

            SpawnAnAnimal();
        }
    }

    /// <summary>
    /// 生成一个动物
    /// </summary>
    public void SpawnAnAnimal()
    {
        //选择一个动物
        int totWeight = 0;
        foreach (var setting in settings.Values)
        {
            totWeight += setting.spawnWeight;
        }
        int choose = Random.Range(0, totWeight);
        string chosenName = string.Empty;
        foreach (var setting in settings.Values)
        {
            if (choose < setting.spawnWeight)
            {
                chosenName = setting.animalName;
                break;
            }
            else
                choose -= setting.spawnWeight;
        }

        GameObject chosenPrefab = null;
        foreach (var prefab in animalPrefabs)
        {
            if (prefab.name == chosenName)
            {
                chosenPrefab = prefab;
                break;
            }
        }

        //选择一个出生点
        if (chosenPrefab != null)
        {
            spawnPlaces[Random.Range(0, spawnPlaces.Count)].Spawn(chosenPrefab);
        }
    }
}
