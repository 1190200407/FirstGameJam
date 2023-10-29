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
    //�����ַ���������д����������������
    public string SceneName;
    private float TargetVaule;
    private UnityEngine.AsyncOperation async = null;
    public TMP_Text tmp;

    private static List<string> StroyInfo = new List<string>();

    static SceneTrans()
    {
        StroyInfo.Add("\"���Ѿ���������������֮ˮ�Ļ������ܡ�\n" +
            "��ʱ������Ҳ�����˾�β԰�����Ǻ�������ã�\n\n" +
            "����ס������֮ˮ��������˵�ǽ��ɡ�רע�����è�䣬" +
            "������������������չ�������ʹʹ����\n\nǰ�����¸ҵ���ʹ��\"");
        StroyInfo.Add("\"�ɵ�Ư����\n"+
            "�ɼ�����Ҳ�����������ʰ�쵵����硣\n" +
            "���ǿ��ٷ��裬���������������µ���ս��\n" +
            "��ס���Ŀ�꣬רע��������֮ˮ������è������������\n\n" +
            "չ�����ӭ���������ս�ɣ�\"");
        StroyInfo.Add("\"���ڣ����Ѿ�������β԰������������ս�������κ�ʱ��Ҫ��\n" +
            "�ɼ��������ͻ�Ծ�����ӣ������Ѿ���һ������ḻ��èè��ʹ��\n" +
            "רע�����ʹ������������֮ˮ������è������������ά����β԰�ĺ�г��ƽ�⡣" +
            "\n\n����ֱǰ��èè��ʹ��\"");
    }
    

    void Start()
    {
        //sliderBar = GameObject.Find("MySliderBar").GetComponent<MySliderBar>();
        UnityEngine.Debug.Log("ʵ����mysliderbar�ɹ�");
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
            //sliderBar.IncreaseTo(TargetVaule);
            //UnityEngine.Debug.Log("��ֵ��slider�ɹ�");
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