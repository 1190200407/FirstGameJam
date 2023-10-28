using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<Button> levelButtons;
    public Button soundButton;
    public Button quitButton;


    void Start()
    {
        //获取UI
        if (levelButtons == null || levelButtons.Count == 0)
        {
            levelButtons = new List<Button>();
            for (int i = 0; i < 4; i++)
                levelButtons.Add(GameObject.Find("levelBtn" + i.ToString()).GetComponent<Button>());
        }

        soundButton ??= GameObject.Find("SoundBtn").GetComponent<Button>();
        quitButton ??= GameObject.Find("QuitBtn").GetComponent<Button>();

        SetLevelButtons();

        //添加监听
        soundButton.onClick.AddListener(SwitchSound);
        quitButton.onClick.AddListener(QuitGame);
        foreach (var button in levelButtons)
            button.onClick.AddListener(()=>
            {
                EnterLevel(levelButtons.IndexOf(button));
            });
    }

    /// <summary>
    /// 根据玩家通关情况设置按钮
    /// </summary>
    public void SetLevelButtons()
    {
        int nowLevel = PlayerPrefs.GetInt("ClearLevels", 0);
        for (int i = 0; i < levelButtons.Count; i++)
            levelButtons[i].interactable = i <= nowLevel;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwitchSound()
    {
    }

    public void EnterLevel(int levelNum)
    {
        SceneManager.LoadScene("LevelScene" + levelNum.ToString());
    }
}
