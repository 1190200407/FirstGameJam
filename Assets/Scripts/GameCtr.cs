using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;

/// <summary>
/// 游戏中枢
/// </summary>
public class GameCtr : MonoBehaviour
{
    public static GameCtr instance;
    public PauseMenu pauseMenu;
    public WinMenu winMenu;
    public Scene currentScene { get; private set; }

    public WaterCtr waterCtr;
    public FollowingMouse followingMouse;
    public DrawAimLine drawAimLine;
    public EnergyBar energyBar;

    public UIMgr uiMgr;
    public AnimalMgr anmMgr;

    private int _nowScore = 0;

    public int NowScore
    { 
        get { return _nowScore; } 
        set 
        {
            uiMgr.OnScoreChange(value);

            if (_nowScore != goalScore)
            {
                _nowScore = value;
                if (_nowScore == goalScore)
                {
                    StartCoroutine(OnLevelSuccess());
                }
            }
        }
    }

    [Header("关卡号")]
    public int levelNum;
    [Header("目标分数")]
    public int goalScore = 1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    IEnumerator Start()
    {
        currentScene = SceneManager.GetActiveScene();
        waterCtr ??= GameObject.Find("WaterStart").GetComponent<WaterCtr>();
        followingMouse ??= GameObject.Find("WaterPot").GetComponent<FollowingMouse>();
        drawAimLine ??= GameObject.Find("Aimline").GetComponent<DrawAimLine>();
        energyBar ??= GameObject.Find("EnergyBar").GetComponent<EnergyBar>();
        uiMgr ??= GetComponent<UIMgr>();
        anmMgr ??= GetComponent<AnimalMgr>();

        pauseMenu ??= GetComponent<PauseMenu>();
        winMenu ??= GetComponent<WinMenu>();
        pauseMenu.pauseMenuUI.SetActive(false);

        yield return null;
        Init();
    }


    private void Init()
    {
        NowScore = 0;
        uiMgr.OnGoalChange(goalScore);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            waterCtr.StartPouring();
        }
        if (Input.GetMouseButtonUp(0) && waterCtr.IsOpen) 
        {
            waterCtr.EndPouring();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiMgr.pauseMenu.PauseOrResume();
        }
    }

    public void Quit()
    {
        pauseMenu.Pause();
    }

    public IEnumerator OnLevelSuccess()
    {
        pauseMenu.CanPause = false;
        yield return new WaitForSecondsRealtime(2.5f);

        PlayerPrefs.SetInt("ClearLevels", levelNum + 1);
        winMenu.OpenWinMenu();

    }

    public IEnumerator OnLevelFail()
    {
        pauseMenu.CanPause = false;
        yield return new WaitForSecondsRealtime(2.5f);

        PlayerPrefs.SetInt("ClearLevels", levelNum + 1);
        winMenu.OpenFailMenu();
    }
}
