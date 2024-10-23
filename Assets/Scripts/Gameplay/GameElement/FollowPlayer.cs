using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool followX;
    public bool followY;
    private PlayerMovement player;

    void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(followX ? player.transform.position.x : transform.position.x, 
                followY ? Mathf.Max(player.transform.position.y, -0.3f) : transform.position.y, transform.position.z);
        }
        else
        {
            player = LevelManager.Instance.player;
        }
    }
}
