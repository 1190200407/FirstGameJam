using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class WinMenu : MonoBehaviour
{
    public Button nextBtn;
    public Button restartBtn;
    public Button quitBtn;
    public Button menuBtn;
    // Start is called before the first frame update
    void Start()
    {
        /*nextBtn ??= GameObject.Find("NextBtn").GetComponent<Button>();
        restartBtn ??= GameObject.Find("RestartBtn").GetComponent<Button>();
        quitBtn ??= GameObject.Find("QuitBtn").GetComponent<Button>();
        menuBtn ??= GameObject.Find("MenuBtn").GetComponent<Button>();

        //ÃÌº”º‡Ã˝
        nextBtn.onClick.AddListener(NextLevel);
        restartBtn.onClick.AddListener(Restart);
        quitBtn.onClick.AddListener(QuitGame);
        menuBtn.onClick.AddListener(LoadMenu);*/
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;//ª∫≥Â ±º‰ª÷∏¥”Œœ∑
        //SceneManager.LoadScene("LevelScene");
        int nowLevel = PlayerPrefs.GetInt("ClearLevels", 0);
        int nextLevel = nowLevel + 1;
        string name = "LevelScene" + nextLevel.ToString();
        UnityEngine.Debug.Log(name);
        SceneManager.LoadScene(name);
    }
    public void Restart()
    {
        UnityEngine.Debug.Log("restart current game...");
        Time.timeScale = 1f;//ª∫≥Â ±º‰ª÷∏¥”Œœ∑
        SceneManager.LoadScene(GameCtr.instance.currentScene.name);
    }
    public void QuitGame()
    {
        UnityEngine.Application.Quit();
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("UIDemo");
    }
}
