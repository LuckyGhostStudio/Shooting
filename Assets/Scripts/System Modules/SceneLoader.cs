using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private Image transitionImage;     //场景切换图片
    [SerializeField] private float fadeTime = 3.5f;     //淡入淡出时间

    private Color color;

    private const string MAIN_MENU = "MainMenu";
    private const string GAMEPLAY = "Gameplay";

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <returns></returns>
    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);  //场景异步加载
        loadingOperation.allowSceneActivation = false;      //场景不激活

        transitionImage.gameObject.SetActive(true);

        //场景淡出
        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);   //增加图片透明度：限制范围[0, 1]
            transitionImage.color = color;

            yield return null;
        }

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);    //等待直到场景加载90%以上

        loadingOperation.allowSceneActivation = true;   //激活场景

        //场景淡入
        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);   //增加图片透明度：限制范围[0, 1]
            transitionImage.color = color;

            yield return null;
        }

        transitionImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 加载Gameplay场景
    /// </summary>
    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));    //加载Gameplay场景
    }

    /// <summary>
    /// 加载主菜单场景
    /// </summary>
    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));    //加载主菜单场景
    }
}
