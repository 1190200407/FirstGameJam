using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button soundButton;
    //调用mainMenu中的isMuOn？应该从调用UIcountrol中调用
    public static bool isMuOn = true;
    public Sprite muOnImg;
    public Sprite muOffImg;
    void Start()
    {
        soundButton ??= GameObject.Find("MusOnBtn").GetComponent<Button>();
        soundButton.onClick.AddListener(SwitchSound);
    }
    public void SwitchSound()
    {
        if (isMuOn)
        {
            isMuOn = false;
            //shut down music//
            UnityEngine.Debug.Log("shut down music" + isMuOn);
            soundButton.GetComponent<UnityEngine.UI.Image>().sprite = muOffImg;
            return;
        }
        else
        {
            isMuOn = true;
            //turn on music//
            UnityEngine.Debug.Log("turn on music" + isMuOn);
            soundButton.GetComponent<UnityEngine.UI.Image>().sprite = muOnImg;
            return;
        }

    }
}
