using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Animal animal) && !animal.isLeaving)
        {
            animal.Direction = -animal.Direction;
        }
    }
}
