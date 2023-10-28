using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
<<<<<<< HEAD:Assets/Scripts/Menu&Scene/MainMenu.cs
    public GameObject settingMenuUI;
    //public GameObject pauseMenuUI;

    bool isLevel0Pass = false;
    bool isLevel1Pass = false;
    bool isLevel2Pass = false;
    bool isLevel3Pass = false;
=======
    public List<Button> levelButtons;
    public Button soundButton;
    public Button quitButton;
>>>>>>> 1e8420077d650d4b2f4c3d24da4c50a4092f7b0c:Assets/Scripts/Menu/MainMenu.cs


    void Start()
    {
<<<<<<< HEAD:Assets/Scripts/Menu&Scene/MainMenu.cs
        settingMenuUI.SetActive(false);
        //pauseMenuUI.SetActive(false);
=======
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
>>>>>>> 1e8420077d650d4b2f4c3d24da4c50a4092f7b0c:Assets/Scripts/Menu/MainMenu.cs
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
<<<<<<< HEAD:Assets/Scripts/Menu&Scene/MainMenu.cs
        Time.timeScale = 1f;//缓冲时间恢复游戏
        SceneManager.LoadScene("LevelScene");
=======
        SceneManager.LoadScene("LevelScene" + levelNum.ToString());
>>>>>>> 1e8420077d650d4b2f4c3d24da4c50a4092f7b0c:Assets/Scripts/Menu/MainMenu.cs
    }
}
