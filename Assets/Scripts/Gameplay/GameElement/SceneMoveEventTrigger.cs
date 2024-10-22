using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SceneMoveEventTrigger : EventTrigger
{
    public Transform objToMove;
    public Vector3 upPos;
    public Vector3 downPos;
    public float moveTime;
    public Ease moveEase;

    public override void OnPlayerExit()
    {
        if (LevelManager.Instance.player.transform.position.y > transform.position.y)
        {
            objToMove.DOMove(upPos, moveTime).SetEase(moveEase);
        }
        else
        {
            objToMove.DOMove(downPos, moveTime).SetEase(moveEase);
        }
    }
}
