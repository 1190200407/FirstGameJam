using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    public void Restart()
    {
        LevelManager.Instance.Restart();
    }

    public void MainMenu()
    {
        LevelManager.Instance.MainMenu();
    }
}
