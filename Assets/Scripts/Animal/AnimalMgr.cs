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
    /// ����һ������
    /// </summary>
    public void SpawnAnAnimal()
    {
        //ѡ��һ������
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

        //ѡ��һ��������
        if (chosenPrefab != null)
        {
            spawnPlaces[Random.Range(0, spawnPlaces.Count)].Spawn(chosenPrefab);
        }
    }
}
