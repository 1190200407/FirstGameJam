using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public string levelId;
    public string levelName;
    public string levelDesc;
    public GameObject levelMap;
    public List<GameItemData> gameItems;
    public Sprite innerBackGround;
    public Sprite outsideBackGround;
    public Sprite frame;
    public Color themeColor;
    public AudioClip backgroundMusic;
}
