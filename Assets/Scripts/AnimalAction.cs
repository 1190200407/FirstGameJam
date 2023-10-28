using UnityEngine;

public class AnimalAction
{
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