using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoSingleton<LevelManager>
{
    public bool testMode = false;
    public LevelData levelData;
    public PlayerMovement player;
    public InputHandler input;
    public GameObject levelMap = null;
    
    public Light2D innerGlobalLight {get; private set;}
    public Light2D outsideGlobalLight {get; private set;}
    public Transform lightParent {get; private set;}
    public List<ShadowCaster2D> ShadowCasters {get; private set;}
    public List<Mirror> Mirrors {get; private set;}

    public SpriteRenderer outsideBackground {get; private set;}
    
    public SpriteRenderer innerBackground {get; private set;}
    
    public SpriteRenderer oursideFrame {get; private set;}
    public SpriteRenderer outsideLightEdge {get; private set;}
    
    [Header("光线预制体")]
    public FanshapedLight fanLight;
    public CircleLight circleLight;

    [Header("视觉参数")]
    [SerializeField]
    private float changeLightSpeed = 5f;
    [SerializeField]
    private float outsideBrightIntensity = 1f;
    [SerializeField]
    private float outsideDarkIntensity = 0.5f;
    [SerializeField]
    private Color outsideBrightColor;
    [SerializeField]
    private Color outsideDarkColor;
    [SerializeField]
    private float innerBrightIntensity = 2f;
    [SerializeField]
    private float innerDarkIntensity = 1f;

    private Coroutine changeStateCoroutine;

    void OnEnable()
    {
        MyEventSystem.Register<StateChangeEvent>(OnChangeState);
        MyEventSystem.Register<DestroyObjectEvent>(OnDestroyObject);
    }

    void OnDisable()
    {
        MyEventSystem.Unregister<StateChangeEvent>(OnChangeState);
        MyEventSystem.Unregister<DestroyObjectEvent>(OnDestroyObject);
    }

    #region 加载关卡
    public void LoadLevelData(LevelData data)
    {
        levelData = data;
        StartCoroutine(LoadLevelDataCoroutine(data));
    }

    private IEnumerator LoadLevelDataCoroutine(LevelData data)
    {
        //加载关卡背景
        Transform backgroundParent = GameObject.Find("BackGrounds").transform;
        for (int i = 0; i < backgroundParent.childCount; i++)
        {
            Transform bg = backgroundParent.GetChild(i);
            switch (bg.name)
            {
                case "InnerWorld":
                    innerBackground = bg.GetComponent<SpriteRenderer>();
                    innerBackground.sprite = data.innerBackGround;
                    break;
                case "OuterWorld":
                    outsideBackground = bg.GetComponent<SpriteRenderer>();
                    outsideBackground.sprite = data.outsideBackGround;
                    break;
                case "Frame":
                    oursideFrame = bg.GetComponent<SpriteRenderer>();
                    oursideFrame.sprite = data.frame;
                    break;
                case "LightEdge":
                    outsideLightEdge = bg.GetComponent<SpriteRenderer>();
                    outsideLightEdge.material.SetColor("_Color", data.themeColor);
                    break;
                default:
                    Debug.LogError("不支持的名字：" + bg.name);
                    break;
            }
        }

        //加载关卡地图
        levelMap = GameObject.Instantiate(data.levelMap);

        //加载背景音乐
        AudioManager.Instance.ChangeMusic(data.backgroundMusic);

        LoadLevelResources();
        player.GetComponentInChildren<PlayerAnimator>().SetOutlineColor(data.themeColor);
        player.GetComponent<PlayerInventory>().LoadItemDatas(data.gameItems);

        yield return null;
        //开启玩家输入
        player.Init();
    }

    public void LoadLevelResources()
    {
        // 获取玩家
        player = GameObject.FindObjectOfType<PlayerMovement>();
        input = GameObject.FindObjectOfType<InputHandler>();

        // 获取所有光源、遮挡物和镜子
        innerGlobalLight = GameObject.Find("InnerGlobalLight").GetComponent<Light2D>();
        outsideGlobalLight = GameObject.Find("OutsideGlobalLight").GetComponent<Light2D>();
        outsideBackground = GameObject.Find("OuterWorld").GetComponent<SpriteRenderer>();
        lightParent = GameObject.Find("LevelLights").transform;

        ShadowCasters = FindObjectsOfType<ShadowCaster2D>().ToList();
        Mirrors = FindObjectsOfType<Mirror>().ToList();
    }
    #endregion

    #region 事件监听
    private void OnChangeState(StateChangeEvent @event)
    {
        if (changeStateCoroutine != null) StopCoroutine(changeStateCoroutine);
        changeStateCoroutine = StartCoroutine(ChangeStateCoroutine(@event.newState));
    }

    private void OnDestroyObject(DestroyObjectEvent @event)
    {
        if (@event.gameObject.TryGetComponent(out ShadowCaster2D shadowCaster))
        {
            ShadowCasters.Remove(shadowCaster);
        }
    }
    #endregion

    private IEnumerator ChangeStateCoroutine(PlayerState finalState)
    {
        float lerpAmount = 0f;
        float outsideStartIntensity = outsideGlobalLight.intensity;
        Color outsideStartColor = outsideBackground.material.GetColor("_LightColor");
        float innerStartIntensity = innerGlobalLight.intensity;
        
        float outsideFinalIntensity = finalState == PlayerState.Normal ? outsideBrightIntensity : outsideDarkIntensity;
        Color outsideFinalColor = finalState == PlayerState.Normal ? outsideBrightColor : outsideDarkColor;
        float innerFinalIntensity = finalState == PlayerState.Normal ? innerDarkIntensity : innerBrightIntensity;

        do
        {
            yield return null;

            lerpAmount += Time.deltaTime * changeLightSpeed;
            outsideGlobalLight.intensity = Mathf.Lerp(outsideStartIntensity, outsideFinalIntensity, lerpAmount);
            outsideBackground.material.SetColor("_LightColor", Color.Lerp(outsideStartColor, outsideFinalColor, lerpAmount));
            innerGlobalLight.intensity = Mathf.Lerp(innerStartIntensity, innerFinalIntensity, lerpAmount);
        }
        while (lerpAmount < 1f);
    }

    #region 创造光源
    public FanshapedLight MakeFanshapedLight()
    {
        FanshapedLight light = Instantiate(fanLight, lightParent);
        return light;
    }

    public CircleLight MakeCircleLight()
    {
        CircleLight light = Instantiate(circleLight, lightParent);
        return light;
    }
    #endregion

    #region 关卡操作
    public void Restart()
    {
        SceneLoadManager.Instance.LoadLevel(1, levelData);
    }

    public void MainMenu()
    {
        SceneLoadManager.Instance.LoadLevel(0, null);
    }
    #endregion
}
