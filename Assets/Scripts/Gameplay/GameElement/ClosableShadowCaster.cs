using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosableShadowCaster : MonoBehaviour
{
    public void Close()
    {
        MyEventSystem.Trigger(new DestroyObjectEvent { gameObject = this.gameObject });
    }

    public void Open()
    {
        MyEventSystem.Trigger(new CreateObjectEvent { gameObject = this.gameObject });
    }
}
