using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtr : MonoBehaviour
{
    public WaterCtr waterCtr;
    public FollowingMouse followingMouse;

    void Start()
    {
        waterCtr ??= GameObject.Find("WaterPot").GetComponent<WaterCtr>();
        followingMouse ??= GameObject.Find("WaterPot").GetComponent<FollowingMouse>();
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
    }
}
