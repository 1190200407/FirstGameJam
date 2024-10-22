using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAppearEventTrigger : EventTrigger
{
    public GameObject enemy;
    public AudioClip enemyBgm;

    void Start()
    {
        enemy.gameObject.SetActive(false);
    }

    public override void OnPlayerEnter()
    {
        enemy.gameObject.SetActive(true);
        AudioManager.Instance.ChangeMusic(enemyBgm);
        AudioManager.Instance.PlaySfx("怪物出现");
    }
}
