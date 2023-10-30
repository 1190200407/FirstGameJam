using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Addition : MonoBehaviour
{
    IEnumerator Start()
    {
        //��Ҫ��Ģ���ͻ����̫��
        List<GameObject> list = new List<GameObject>();
        list.Add(GameObject.Find("Flower"));
        list.Add(GameObject.Find("Mushroom"));
        list.Add(GameObject.Find("Mushroom2"));
        bool pass;
        do
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].transform.position = new Vector3(Random.Range(-8f, 8f), -3.6f, 0f);
            }

            pass = true;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if ((list[i].transform.position - list[j].transform.position).magnitude < 4f)
                    {
                        pass = false;
                        break;
                    }
                }
                if (!pass) break;
            }
        } while (!pass);

        //�ڵ����أ���10��30��60�������һֻ������
        for (int i = 0; i <= 3; i++)
        {
            yield return new WaitForSeconds(10f * i);
            GameCtr.instance.anmMgr.SpawnAnAnimal("Chick");
        }
    }
}
