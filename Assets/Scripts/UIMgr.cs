using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ¸ºÔð¿ØÖÆUI
/// </summary>
public class UIMgr : MonoBehaviour
{
    public Text nowScoreText;
    public Text goalScoreText;

    public PauseMenu pauseMenu;

    private void Start()
    {
        nowScoreText ??= GameObject.Find("NowScore").GetComponent<Text>();
        goalScoreText ??= GameObject.Find("GoalScore").GetComponent<Text>();
        pauseMenu ??= GetComponent<PauseMenu>();
    }

    public void OnScoreChange(int score)
    {
        nowScoreText.text = $"{score:d6}";
    }

    public void OnGoalChange(int score)
    {
        goalScoreText.text = $"{score:d6}";
    }
}
