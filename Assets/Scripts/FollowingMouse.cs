using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingMouse : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
    }
}
