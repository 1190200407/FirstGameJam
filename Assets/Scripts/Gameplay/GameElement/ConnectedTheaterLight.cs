using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedTheaterLight : TheaterLight
{
    void OnEnable()
    {
        MyEventSystem.Register<TheaterLightMoveEvent>(OnTheaterLightMove);
    }

    void OnDisable()
    {
        MyEventSystem.Unregister<TheaterLightMoveEvent>(OnTheaterLightMove);
    }

    private void OnTheaterLightMove(TheaterLightMoveEvent @event)
    {
        if (@event.from != this)
        {
            Move(@event.delta);
        }
    }
}
