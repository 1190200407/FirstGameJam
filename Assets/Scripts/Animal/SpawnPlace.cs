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
    public void Spawn(GameObject prefab)
    {
        Animal animal = Instantiate(prefab, transform.position, Quaternion.identity, animalParent).GetComponent<Animal>();
        animal.Direction = direction;
        animal.Init();
    }
}