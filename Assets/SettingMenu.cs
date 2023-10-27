using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingMenuUI;

    static bool isMusicOn = true;

    public static bool currentMusic { get { return isMusicOn; } }

    public void Music()
    {
        if (isMusicOn)
        {
            UnityEngine.Debug.Log("Turn off music");
            isMusicOn = false;
        }
        else
        {
            UnityEngine.Debug.Log("Turn on music");
            isMusicOn = true;
        }
        
    }
    public void Return()
    {
        settingMenuUI.SetActive(false);
    }

}
