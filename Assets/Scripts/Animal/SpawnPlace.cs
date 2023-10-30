using UnityEngine;

/// <summary>
/// ���ɵ�
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
    /// ����һֻ����
    /// </summary>
    /// <param name="prefab">�����Ԥ����</param>
    /// <param name="name">������</param>
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