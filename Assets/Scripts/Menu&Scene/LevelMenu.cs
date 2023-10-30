using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button soundButton;
    public Image soundImage;
    public Sprite muOnImg;
    public Sprite muOffImg;
    public static bool isMuOn = true;

    private void Start()
    {
        isMuOn = PlayerPrefs.GetInt("isMuOn", 0) == 0;
        soundButton ??= GameObject.Find("MuOnBtn").GetComponent<Button>();
        soundImage = soundButton.GetComponent<Image>();
        soundImage.sprite = isMuOn ? muOnImg : muOffImg;
        soundButton.onClick.AddListener(SwitchSound);

        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            source.enabled = isMuOn;
        }
    }

    public void SwitchSound()
    {
        if (isMuOn)
        {
            isMuOn = false;
            PlayerPrefs.SetInt("isMuOn", 1);
            soundImage.sprite = muOffImg;
        }
        else
        {
            isMuOn = true;
            PlayerPrefs.SetInt("isMuOn", 0);
            soundImage.sprite = muOnImg;
        }

        PlayerPrefs.SetInt("IsMuOn", isMuOn ? 0 : 1);
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (isMuOn && !source.enabled)
            {
                source.enabled = true;
            }
            source.volume = isMuOn ? 1f : 0f;
        }
    }
}
