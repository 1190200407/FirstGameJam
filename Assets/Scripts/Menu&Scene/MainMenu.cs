using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class MainMenu : MonoBehaviour
{
    public List<Button> levelButtons;
    public Button soundButton;
    public Button quitButton;
    public Sprite muOnImg;
    public Sprite muOffImg;
    public static bool isMuOn = true;

    public AudioSource bgAudioSource;
    public AudioSource sfxAudioSource;

    void Start()
    {
        //获取UI
        if (levelButtons == null || levelButtons.Count == 0)
        {
            levelButtons = new List<Button>();
            for (int i = 0; i < 4; i++)
                levelButtons.Add(GameObject.Find("levelBtn" + i.ToString()).GetComponent<Button>());
        }
        soundButton ??= GameObject.Find("MusicBtn").GetComponent<Button>();

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

        isMuOn = PlayerPrefs.GetInt("IsMuOn", 0) == 0;
        soundButton.GetComponent<UnityEngine.UI.Image>().sprite = isMuOn ? muOnImg : muOffImg;
        bgAudioSource.enabled = isMuOn;
        sfxAudioSource.enabled = isMuOn;
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
            soundButton.GetComponent<UnityEngine.UI.Image>().sprite = muOffImg;
        }
        else
        {
            isMuOn = true;
            soundButton.GetComponent<UnityEngine.UI.Image>().sprite = muOnImg;
        }

        PlayerPrefs.SetInt("IsMuOn", isMuOn ? 0 : 1);
        if (isMuOn && !bgAudioSource.enabled)
        {
            bgAudioSource.enabled = true;
        }
        bgAudioSource.volume = isMuOn ? 1f : 0f;
        sfxAudioSource.enabled = isMuOn;
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
