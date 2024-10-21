using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : GameItemScript
{
    public FanshapedLight light;
    public PlayerMovement player;
    public bool turnOn = false;
    public bool selected = false;
    public static Vector3[] PointDirs = {
        new Vector2(0.5f, 0.2f), new Vector2(0.5f, -0.2f), new Vector2(-0.5f, -0.2f), new Vector2(-0.5f, 0.2f), new Vector2(-100f, 0f), new Vector2(100f, 0f)
    };
    private Vector3 lastPostion;
    private bool lastFacing;

    public override void OnCreate()
    {
        light = LevelManager.Instance.MakeFanshapedLight();
        light.gameObject.SetActive(false);
        player = LevelManager.Instance.player;
    }

    public override void OnSelect()
    {
        selected = true;
        light.gameObject.SetActive(turnOn);
    }

    public override void OnUnselect()
    {
        selected = false;
        light.gameObject.SetActive(false);
    }

    public override void OnActivate()
    {
        turnOn = !turnOn;
        if (turnOn && selected)
            light.gameObject.SetActive(true);
        else
            light.gameObject.SetActive(false);
    }

    public override void OnHold()
    {
        //调整光源位置
        if (player.IsFacingRight != lastFacing || Vector2.SqrMagnitude(player.transform.position - lastPostion) > 0.1f)
        {
            light.cutPoint1.position = player.transform.position + PointDirs[player.IsFacingRight ? 0 : 2];
            light.cutPoint2.position = player.transform.position + PointDirs[player.IsFacingRight ? 1 : 3];
            light.startPoint.position = player.transform.position + PointDirs[player.IsFacingRight ? 4 : 5];
            lastPostion = player.transform.position;
        }

        lastFacing = player.IsFacingRight;
    }
}
