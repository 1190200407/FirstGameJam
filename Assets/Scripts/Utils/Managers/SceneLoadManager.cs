using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    public Animator transition;

    private int buildIndex;
    private LevelData dataToload;

    public void LoadLevel(int buildIndex, LevelData dataToLoad)
    {
        transition.Play("Close");
        this.buildIndex = buildIndex;
        this.dataToload = dataToLoad;
    }

    public void OnCloseAnimtionEnd()
    {
        StartCoroutine(LoadLevelCoroutine());
    }

    IEnumerator LoadLevelCoroutine()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        if (dataToload != null)
            LevelManager.Instance.LoadLevelData(dataToload);

        transition.Play("Open");
    }
}
