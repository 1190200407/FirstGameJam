using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioSource musicSource1;  // 第一个音乐播放器
    public AudioSource musicSource2;  // 第二个音乐播放器
    public AudioSource sfxAudioSrc;

    private AudioSource activeSource; // 当前正在播放的播放器
    public float fadeDuration = 2f;   // 渐变持续时间
    public float targetVolume = 1f;   // 目标音量

    public List<AudioClip> bgmList;
    public List<AudioClip> sfxList;

    private Dictionary<string, float> sfxCdDict;
    private Dictionary<string, AudioClip> nameSfxDict;

    protected override void Awake()
    {
        base.Awake();
        sfxCdDict = new Dictionary<string, float>();
        nameSfxDict = new Dictionary<string, AudioClip>();
        foreach (var sfx in sfxList)
        {
            nameSfxDict.Add(sfx.name, sfx);
        }
    }

    private void Start()
    {
        activeSource = musicSource1;  // 设置初始的活跃音源
    }

    private void Update()
    {
        foreach (var key in sfxList)
        {
            string name = key.name;
            if (sfxCdDict.ContainsKey(name) && sfxCdDict[name] > 0f)
                sfxCdDict[name] -= Time.deltaTime;
        }
    }
    
    // 切换到新的背景音乐
    public void ChangeMusic(AudioClip newClip)
    {
    // 如果当前音源正在播放相同的音频剪辑，则不进行切换
    if (activeSource != null && activeSource.clip == newClip)
    {
        return;
    }

    // 判断当前活跃的音源
    AudioSource nextSource = (activeSource == null || activeSource == musicSource2) ? musicSource1 : musicSource2;

    // 设置新的音频剪辑到空闲的播放器上
    nextSource.clip = newClip;

    // 渐变淡出当前音源到一定的音量
    AudioSource lastSource = activeSource;
    if (activeSource != null)
    {
        activeSource.DOFade(0.2f, fadeDuration / 2).OnComplete(() =>
        {
            // 渐出完成后再完全淡出并停止播放
            activeSource.DOFade(0, fadeDuration / 2).OnComplete(() =>
            {
                lastSource.Stop();  // 完全淡出后停止播放
            });

            // 渐变渐入新音源
            nextSource.volume = 0.5f;    // 设置音量为 0.5，准备渐入
            nextSource.Play();        // 播放新音乐
            nextSource.DOFade(targetVolume, fadeDuration);
        });
    }
    else
    {
        // 如果当前没有活跃音源，直接渐入新音源
        nextSource.volume = 0.5f;    // 设置音量为 0.5，准备渐入
        nextSource.Play();        // 播放新音乐
        nextSource.DOFade(targetVolume, fadeDuration);
    }

    // 切换活跃的播放器
    activeSource = nextSource;
}

    public void StopMusic()
    {
        // 渐变淡出当前音源
        AudioSource lastSource = activeSource;
        if (activeSource != null)
            activeSource.DOFade(0, fadeDuration);

        // 切换活跃的播放器
        activeSource = null;
    }

    public void PlaySfx(string soundName, float volumeScale = 1f)
    {
        //在冷却中
        if (sfxCdDict.ContainsKey(soundName) && sfxCdDict[soundName] > 0f) return;

        if (nameSfxDict.ContainsKey(soundName))
        {
            sfxAudioSrc.PlayOneShot(nameSfxDict[soundName], volumeScale);
        }
        else
        {
            Debug.LogError("没有这个音效");
        }
    }
    
    public void PlaySfxWithCD(string soundName, float cd, float volumeScale = 1f)
    {
        //在冷却中
        if (sfxCdDict.ContainsKey(soundName) && sfxCdDict[soundName] > 0f) return;
        //设置冷却
        sfxCdDict[soundName] = cd;
        
        if (nameSfxDict.ContainsKey(soundName))
        {
            sfxAudioSrc.PlayOneShot(nameSfxDict[soundName], volumeScale);
        }
        else
        {
            Debug.LogError("没有这个音效");
        }
    }
}
