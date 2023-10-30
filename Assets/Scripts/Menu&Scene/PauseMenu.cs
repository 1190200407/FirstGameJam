using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    private bool _canPause = true;
    public bool CanPause
    { 
        get { return _canPause; } 
        set 
        { 
            _canPause = value; 
            if (!value && isGamePaused)
            {
                Resume();
            }
        }
    }
    private static string mainMenu = "StartMenu";
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
        Time.timeScale = 1f;//ª∫≥Â ±º‰ª÷∏¥”Œœ∑
        isGamePaused = false;
    }

    public void Pause()
    {
        if (!_canPause) return;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;//¡¢øÃ‘›Õ£”Œœ∑
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void Restart()
    {
        UnityEngine.Debug.Log("restart current game...");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;//ª∫≥Â ±º‰ª÷∏¥”Œœ∑
        isGamePaused = false;
        SceneManager.LoadScene(GameCtr.instance.currentScene.name);
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("quitting game...");
        Application.Quit();
    }
}
