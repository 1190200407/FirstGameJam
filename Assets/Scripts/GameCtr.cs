using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtr : MonoBehaviour
{
    public static GameCtr instance;

    public WaterCtr waterCtr;
    public FollowingMouse followingMouse;
    public DrawAimLine drawAimLine;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    void Start()
    {
        waterCtr ??= GameObject.Find("WaterPot").GetComponent<WaterCtr>();
        followingMouse ??= GameObject.Find("WaterPot").GetComponent<FollowingMouse>();
        drawAimLine ??= GameObject.Find("Aimline").GetComponent<DrawAimLine>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            waterCtr.StartPouring();
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            waterCtr.EndPouring();
        }

        if (Input.GetMouseButtonDown(1))
        {
            drawAimLine.IsAiming = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            drawAimLine.IsAiming = false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
