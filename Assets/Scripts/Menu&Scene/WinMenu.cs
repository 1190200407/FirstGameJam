using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class WinMenu : MonoBehaviour
{
    public GameObject WinMenuUI;
    public GameObject FailMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        WinMenuUI.SetActive(false);
        FailMenuUI.SetActive(false);
    }

    public void OpenWinMenu()
    {
        WinMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OpenFailMenu()
    {
        FailMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;//ª∫≥Â ±º‰ª÷∏¥”Œœ∑
        //SceneManager.LoadScene("LevelScene");
        int nowLevel = GameCtr.instance.levelNum;
        if (nowLevel != 3)
        {
            int nextLevel = nowLevel + 1;
            name = "LevelScene" + nextLevel.ToString();
            SceneManager.LoadScene(name);
        }
        else
        {
            GameCtr.instance.pauseMenu.LoadMenu();
        }
    }
}
