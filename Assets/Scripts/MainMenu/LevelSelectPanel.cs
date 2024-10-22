using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPanel : MonoBehaviour
{
    public List<LevelData> levelDatas;
    private Button[] levelButtons;

    void Start()
    {
        levelButtons = GetComponentsInChildren<Button>();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i;
            levelButtons[i].onClick.AddListener(() => { OnButtonClick(level); });
        }
    }

    public void OnButtonClick(int id)
    {
        SceneLoadManager.Instance.LoadLevel(1, levelDatas[id]);
        foreach (var button in levelButtons)
        {
            button.enabled = false;
        }
    }
}
