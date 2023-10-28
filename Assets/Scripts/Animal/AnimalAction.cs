using System.Collections.Generic;
using UnityEngine;

public class AnimalAction
{
    public AnimalSetting Setting => host.Setting;
    public Animal host;
    public bool isEnd = false;
    public virtual void OnActionStart()
    {
    }

    public virtual void OnActionEnd() 
    {
    }

    public virtual void Act()
    {
    }
}