using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AStarScan : MonoBehaviour
{
    public AstarPath astarPath;
    public float scanTime = 1f;
    private float timer = 0;

    void Start()
    {
        timer = scanTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = scanTime;
                astarPath.Scan();
            }
        }
    }
}
