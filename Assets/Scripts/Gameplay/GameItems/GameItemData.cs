using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameItem")]
public class GameItemData : ScriptableObject
{
    public Sprite icon;
    public string desc;
    public bool hasNumberLimit;
    public bool isEmpty;
    public string scriptName;
    public GameObject relativePrefab;
}
