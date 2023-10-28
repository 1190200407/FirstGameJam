using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    private static string mainMenu = "UIDemo";
    public GameObject pauseMenuUI;

    private void Start()
    {
        Resume();
    }

    public void PauseOrResume()
    {
        if (!isGamePaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;//����ʱ��ָ���Ϸ
        isGamePaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;//������ͣ��Ϸ
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("UIDemo");
    }

    public void Restart()
    {

        UnityEngine.Debug.Log("restart current game...");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;//����ʱ��ָ���Ϸ
        isGamePaused = false;
        SceneManager.LoadScene(GameCtr.instance.currentScene.name);
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("quitting game...");
        Application.Quit();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
