using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class SceneTrans : MonoBehaviour
{
    public MySliderBar sliderBar;
    //公开字符串用于填写第三个场景的名称
    public string SceneName;
    private float TargetVaule;
    private UnityEngine.AsyncOperation async = null;

    void Start()
    {
        sliderBar = GameObject.Find("MySliderBar").GetComponent<MySliderBar>();
        UnityEngine.Debug.Log("实例化mysliderbar成功");
        StartCoroutine(AsyncLoading());
    }

    void Update()
    {
        //sliderBar.Increase(1000f * Time.deltaTime);
        //UnityEngine.Debug.Log(sliderBar.Value());
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
            sliderBar.IncreaseTo(TargetVaule);
            UnityEngine.Debug.Log("赋值给slider成功");
            UnityEngine.Debug.Log(TargetVaule);
            if (TargetVaule >= 0.9)
            {
                async.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(1.0f);

        }
    }
}