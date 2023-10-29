using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class SceneTrans : MonoBehaviour
{
    //public MySliderBar sliderBar;
    //公开字符串用于填写第三个场景的名称
    public string SceneName;
    private float TargetVaule;
    private UnityEngine.AsyncOperation async = null;
    public TMP_Text tmp;

    private static List<string> StroyInfo = new List<string>();

    static SceneTrans()
    {
        StroyInfo.Add("\"你已经掌握了引导生命之水的基本技能。\n" +
            "此时，狗狗也来到了九尾园，它们好奇而活泼，\n\n" +
            "但记住，生命之水对它们来说是禁忌。专注于你的猫咪，" +
            "引导它吸收生命力，展现你的天使使命。\n\n前进，勇敢的天使！\"");
        StroyInfo.Add("\"干得漂亮！\n"+
            "飞鸡现在也加入了这个五彩斑斓的世界。\n" +
            "它们快速飞翔，给你的任务带来了新的挑战。\n" +
            "记住你的目标，专注引导生命之水，助力猫咪吸收力量。\n\n" +
            "展开翅膀，迎接这个新挑战吧！\"");
        StroyInfo.Add("\"现在，你已经来到九尾园的深处，这里的挑战比以往任何时候都要大。\n" +
            "飞鸡的数量和活跃度增加，但你已经是一名经验丰富的猫猫天使。\n" +
            "专注于你的使命，引导生命之水，助力猫咪吸收力量，维护九尾园的和谐与平衡。" +
            "\n\n勇往直前，猫猫天使！\"");
    }
    

    void Start()
    {
        //sliderBar = GameObject.Find("MySliderBar").GetComponent<MySliderBar>();
        UnityEngine.Debug.Log("实例化mysliderbar成功");
        int i = getCurrentLevel();
        StartCoroutine(AsyncLoading());
        StartCoroutine(WordsShowing());
    }

    int getCurrentLevel()
    {
        return 0;
    }

    IEnumerator AsyncLoading()
    {
        //异步加载场景
        async = SceneManager.LoadSceneAsync(SceneName);
        //阻止当加载完成自动切换
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            UnityEngine.Debug.Log("进入循环");
            if (async.progress < 0.9f)
            {
                TargetVaule = async.progress;
            }
            else 
            {
                TargetVaule = 1.0f;
            }
            //sliderBar.IncreaseTo(TargetVaule);
            //UnityEngine.Debug.Log("赋值给slider成功");
            //UnityEngine.Debug.Log(TargetVaule);
            if (TargetVaule >= 0.9)
            {
                async.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(11.0f);

        }
    }
    IEnumerator WordsShowing()
    {
        yield return null;
    }
}