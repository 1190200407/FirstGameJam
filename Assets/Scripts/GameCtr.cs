using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// ”Œœ∑÷– ‡
/// </summary>
public class GameCtr : MonoBehaviour
{
    public static GameCtr instance;
    public PauseMenu pauseMenu;
    public Scene currentScene { get; private set; }
    public int levelNum;

    public WaterCtr waterCtr;
    public FollowingMouse followingMouse;
    public DrawAimLine drawAimLine;
    public EnergyBar energyBar;

    public UIMgr uiMgr;
    public AnimalMgr anmMgr;

    //public List<bool> isLevelPass = new List<bool>({false, false, false, false});

    private int _nowScore = 0;

    public int NowScore
    { 
        get { return _nowScore; } 
        set 
        {
            _nowScore = value;
            uiMgr.OnScoreChange(value);
        }
    }

    private int _goalScore = 1;
    public int GoalScore
    {
        get { return _goalScore; }
        set
        {
            _goalScore = value;
            uiMgr.OnGoalChange(value);
        }
    }

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
        pauseMenu.pauseMenuUI.SetActive(false);
        currentScene = SceneManager.GetActiveScene();
        waterCtr ??= GameObject.Find("WaterPot").GetComponent<WaterCtr>();
        followingMouse ??= GameObject.Find("WaterPot").GetComponent<FollowingMouse>();
        drawAimLine ??= GameObject.Find("Aimline").GetComponent<DrawAimLine>();
        energyBar ??= GameObject.Find("EnergyBar").GetComponent<EnergyBar>();
        uiMgr ??= GetComponent<UIMgr>();
        anmMgr ??= GetComponent<AnimalMgr>();

        yield return null;
        Init();
    }


    private void Init()
    {
        NowScore = 0;
        GoalScore = 100;
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

    public void OnLevelSuccess()
    {
        UnityEngine.Debug.Log("next level");
        SceneManager.LoadScene("AsyncLoad");
    }

    public void OnLevelFail()
    {

    }
}
