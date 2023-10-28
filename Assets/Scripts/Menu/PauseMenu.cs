using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;//ª∫≥Â ±º‰ª÷∏¥”Œœ∑
        isGamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;//¡¢øÃ‘›Õ£”Œœ∑
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Restart()
    {
        UnityEngine.Debug.Log("restart current game...");
        SceneManager.LoadScene("LevelScene");
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("quitting game...");
        Application.Quit();
    }
}
