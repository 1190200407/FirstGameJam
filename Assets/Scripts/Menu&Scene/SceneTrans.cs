using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class SceneTrans : MonoBehaviour
{
    public MySliderBar sliderBar;
    //�����ַ���������д����������������
    public string SceneName;
    private float TargetVaule;
    private UnityEngine.AsyncOperation async = null;

    void Start()
    {
        sliderBar = GameObject.Find("MySliderBar").GetComponent<MySliderBar>();
        UnityEngine.Debug.Log("ʵ����mysliderbar�ɹ�");
        StartCoroutine(AsyncLoading());
    }

    void Update()
    {
        //sliderBar.Increase(1000f * Time.deltaTime);
        //UnityEngine.Debug.Log(sliderBar.Value());
    }

    IEnumerator AsyncLoading()
    {
        //�첽���س���
        async = SceneManager.LoadSceneAsync(SceneName);
        //��ֹ����������Զ��л�
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            UnityEngine.Debug.Log("����ѭ��");
            if (async.progress < 0.9f)
            {
                TargetVaule = async.progress;
            }
            else 
            {
                TargetVaule = 1.0f;
            }
            sliderBar.IncreaseTo(TargetVaule);
            UnityEngine.Debug.Log("��ֵ��slider�ɹ�");
            UnityEngine.Debug.Log(TargetVaule);
            if (TargetVaule >= 0.9)
            {
                async.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(1.0f);

        }
    }
}