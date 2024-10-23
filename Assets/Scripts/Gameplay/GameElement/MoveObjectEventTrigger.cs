using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveObjectEventTrigger : EventTrigger
{
    public bool triggered = false;
    public bool canTrigger = false;
    public MoveObjectEventTrigger nextTrigger;
    public Transform destPoint;
    public Transform movedObject;

    public override void OnPlayerEnter()
    {
        if (triggered || !canTrigger) return;
        if (nextTrigger != null) nextTrigger.canTrigger = true;
        movedObject.DOMove(destPoint.position, 5f).SetSpeedBased().SetEase(Ease.OutQuint);
    }
}
