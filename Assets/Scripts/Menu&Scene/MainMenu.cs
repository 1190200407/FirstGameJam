using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingMenuUI;
    //public GameObject pauseMenuUI;

    bool isLevel0Pass = false;
    bool isLevel1Pass = false;
    bool isLevel2Pass = false;
    bool isLevel3Pass = false;


    void Start()
    {
        settingMenuUI.SetActive(false);
        //pauseMenuUI.SetActive(false);
    }
    public void QuitGame()
    {
        UnityEngine.Debug.Log("quitting game...");
        Application.Quit();
    }
    public void OpenSetting()
    {
        settingMenuUI.SetActive(true);
    }

    public void EnterLevel0()
    {
        Time.timeScale = 1f;//ª∫≥Â ±º‰ª÷∏¥”Œœ∑
        SceneManager.LoadScene("LevelScene");
    }

    public void EntryLevel1()
    {
        if (isLevel0Pass)
        {
            SceneManager.LoadScene("LevelScene1");
        }
        else
        {
            UnityEngine.Debug.Log("please pass level0 first");
        }
        
    }

    public void EntryLevel2()
    {
        if (isLevel0Pass&&isLevel1Pass)
        {
            SceneManager.LoadScene("LevelScene2");
        }
        else if(isLevel0Pass)
        {
            UnityEngine.Debug.Log("please pass level1 first");
        }else
        {
            UnityEngine.Debug.Log("please pass level0 first");
        }

    }

    public void EntryLevel3()
    {
        if (isLevel0Pass && isLevel1Pass && isLevel2Pass)
        {
            SceneManager.LoadScene("LevelScene3");
        }
        else if (!isLevel0Pass)
        {
            UnityEngine.Debug.Log("please pass level0 first");
        }
        else if(!isLevel1Pass)
        {
            UnityEngine.Debug.Log("please pass level1 first");
        }
        else
        {
            UnityEngine.Debug.Log("please pass level2 first");
        }

    }

}
