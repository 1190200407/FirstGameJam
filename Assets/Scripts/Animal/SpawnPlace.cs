using UnityEngine;

/// <summary>
/// 生成点
/// </summary>
public class SpawnPlace : MonoBehaviour
{
    public int direction;
    public Transform animalParent;

    private void Start()
    {
        animalParent ??= GameObject.Find("TargetZone").transform;
    }

    /// <summary>
    /// 生成一只动物
    /// </summary>
    /// <param name="prefab">动物的预制体</param>
    /// <param name="name">动物名</param>
    public void Spawn(GameObject prefab, string name)
    {
        Animal animal = Instantiate(prefab, transform.position, Quaternion.identity, animalParent).GetComponent<Animal>();
        animal.GetComponent<AudioSource>().enabled = LevelMenu.isMuOn;
        animal.Direction = direction;
        animal.nextAction = Random.Range(0, 2) == 0 ? new RunAction() : new WalkAction();
        animal.name = name;
        animal.Init();
    }
}