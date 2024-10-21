using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFireflyPuzzle : MonoBehaviour
{
    public List<ColorFirefly> answers;
    int cur = 0;
    public GameObject keyObj;

    void OnEnable()
    {
//        LevelManager.Instance.outsideLightEdge.gameObject.SetActive(false);
        keyObj.gameObject.SetActive(false);
        MyEventSystem.Register<TouchFireflyEvent>(OnTouchFirefly);
    }
    
    void OnDisable()
    {
//        LevelManager.Instance.outsideLightEdge.gameObject.SetActive(true);
        MyEventSystem.Unregister<TouchFireflyEvent>(OnTouchFirefly);
    }

    public void OnTouchFirefly(TouchFireflyEvent @event)
    {
        if (cur == answers.Count) return;
        if (@event.colorFirefly != answers[cur])
        {
            cur = 0;
            foreach (var firefly in answers)
            {
                firefly.GoBack();
            }
        }
        else
        {
            @event.colorFirefly.Success();
            cur++;
            if (cur == answers.Count)
            {
                keyObj.SetActive(true);
            }
        }
    }
}
