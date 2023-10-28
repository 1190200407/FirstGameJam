using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{

    //public GameObject settingMenuUI;
    //public GameObject pauseMenuUI;

    bool isLevel0Pass = false;
    bool isLevel1Pass = false;
    bool isLevel2Pass = false;
    bool isLevel3Pass = false;

    public List<Button> levelButtons;
    public Button soundButton;
    public Button quitButton;
    public Sprite muOnImg;
    public Sprite muOffImg;
    public static bool isMuOn = true;

    void Start()
    {

        //settingMenuUI.SetActive(false);
        //pauseMenuUI.SetActive(false);
        //获取UI
        if (levelButtons == null || levelButtons.Count == 0)
        {
            levelButtons = new List<Button>();
            for (int i = 0; i < 4; i++)
                levelButtons.Add(GameObject.Find("levelBtn" + i.ToString()).GetComponent<Button>());
        }
        soundButton ??= GameObject.Find("SoundBtn").GetComponent<Button>();
        quitButton ??= GameObject.Find("QuitBtn").GetComponent<Button>();
        //添加监听
        soundButton.onClick.AddListener(SwitchSound);
        quitButton.onClick.AddListener(QuitGame);

        SetLevelButtons();
        foreach (var button in levelButtons)
            button.onClick.AddListener(() =>
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
        UnityEngine.Application.Quit();
    }

    public void SwitchSound()
    {
        if (isMuOn)
        {
            isMuOn = false;
            //shut down music//
            UnityEngine.Debug.Log("shut down music"+isMuOn);
            soundButton.GetComponent<UnityEngine.UI.Image>().sprite = muOffImg;
            return;
        }
        else
        {
            isMuOn = true;
            //turn on music//
            UnityEngine.Debug.Log("turn on music"+isMuOn);
            soundButton.GetComponent<UnityEngine.UI.Image>().sprite = muOnImg;
            return;
        }
        
    }

    public void EnterLevel(int levelNum)
    {

        Time.timeScale = 1f;//缓冲时间恢复游戏
        //SceneManager.LoadScene("LevelScene");
        string name = "LevelScene" + levelNum.ToString();
        UnityEngine.Debug.Log(name);
        SceneManager.LoadScene(name);

    }

    /*public void EnterSetting()
    {
        settingMenuUI.SetActive(true);
        soundButton ??= GameObject.Find("SoundBtn").GetComponent<Button>();
        quitButton ??= GameObject.Find("QuitBtn").GetComponent<Button>();
        soundButton.onClick.AddListener(SwitchSound);
        quitButton.onClick.AddListener(QuitGame);

    }*/
}
