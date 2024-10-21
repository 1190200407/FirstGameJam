using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ColorFirefly : Firefly
{
    public Color color;
    private Vector3 originalPos;
    private Vector3 successPos;
    public Transform successPoint;
    private float sqrHalfRadius;
    private bool triggered = false;
    private float failSpeed = 5f;
    
    public override void Start()
    {
        light2D.color = color;
        successPos = successPoint.position;
        sqrHalfRadius = radius * radius / 25f;
        originalPos = transform.position;
    }
    
    void Update()
    {
        if (triggered) return;
        if (LevelManager.Instance.player.CurState == PlayerState.Normal) return;
        if (Vector2.SqrMagnitude(transform.position - LevelManager.Instance.player.transform.position) < sqrHalfRadius)
        {
            triggered = true;
            MyEventSystem.Trigger(new TouchFireflyEvent { colorFirefly = this });
        }
    }

    public void Success()
    {
        OnSuccessMoveStart();
        transform.DOMove(successPos, moveSpeed).SetSpeedBased().SetEase(moveEase);
        LevelManager.Instance.player.transform.DOMove(successPos, moveSpeed).SetSpeedBased().SetEase(moveEase).OnComplete(OnSuccessMoveEnd);
    }

    public void GoBack()
    {
        circleLight.collider2D.enabled = false;
        triggered = true;
        transform.DOMove(originalPos, failSpeed).SetSpeedBased().SetEase(moveEase).OnComplete(ResetPuzzle).OnKill(ResetPuzzle);
    }

    public void OnSuccessMoveStart()
    {
        LevelManager.Instance.player.IsInteracting = true;
        LevelManager.Instance.player.RB.velocity = Vector2.zero;
        LevelManager.Instance.player.SetGravityScale(0);
    }

    public void OnSuccessMoveEnd()
    {
        LevelManager.Instance.player.IsInteracting = false;
    }

    public void ResetPuzzle()
    {
        circleLight.collider2D.enabled = true;
        triggered = false;
    }
}
