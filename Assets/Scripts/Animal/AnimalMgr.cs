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
    public Dictionary<string, AnimalSetting> settings = new();
    public Dictionary<string, int> animalCount = new();

    public int constantSameCnt = 0;
    public string lastSpawnName = string.Empty;

    private void Start()
    {
        foreach (var setting in FindObjectsOfType<AnimalSetting>())
        {
            settings[setting.animalName] = setting;
            animalCount[setting.animalName] = 0;
        }

        foreach (var animal in FindObjectsOfType<Animal>())
        {
            animal.nextAction = new LieAction();
            animalCount[animal.name]++;
            animal.Init();
        }
        StartCoroutine(SpawnAnimal());
    }

    private void Update()
    {
        if (animalCount["Cat"] == 0)
        {
            SpawnAnAnimal("Cat");
        }
    }

    private IEnumerator SpawnAnimal()
    {
        while (true)
        {
            float time = Random.Range(-animalSpawnTimeBias, animalSpawnTimeBias) + animalSpawnTime;
            yield return new WaitForSeconds(time);

            SpawnAnAnimal(ChooseAnAnimal());
        }
    }

    public string ChooseAnAnimal()
    {
        //ѡ��һ������
        int totWeight = 0;
        foreach (var setting in settings.Values)
        {
            totWeight += setting.spawnWeight;
        }

        string chosenName = string.Empty;
        do
        {
            int choose = Random.Range(0, totWeight);
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
        } while (GameCtr.instance.levelNum != 0 && chosenName == lastSpawnName && constantSameCnt >= 3);

        return chosenName;
    }

    public GameObject GetPrefabByName(string name)
    {
        GameObject chosenPrefab = null;
        foreach (var prefab in animalPrefabs)
        {
            if (prefab.name == name)
            {
                chosenPrefab = prefab;
                break;
            }
        }
        return chosenPrefab;
    }

    /// <summary>
    /// ����һ������
    /// </summary>
    public void SpawnAnAnimal(string name)
    {
        GameObject prefab = GetPrefabByName(name);
        if (lastSpawnName == name)
        {
            constantSameCnt++;
        }
        else
        {
            lastSpawnName = name;
            constantSameCnt = 0;
        }

        //ѡ��һ��������
        if (prefab != null)
        {
            spawnPlaces[Random.Range(0, spawnPlaces.Count)].Spawn(prefab, name);
            animalCount[name]++;
        }
    }
}
