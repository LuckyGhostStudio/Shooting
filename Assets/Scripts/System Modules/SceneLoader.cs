using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private Image transitionImage;     //�����л�ͼƬ
    [SerializeField] private float fadeTime = 3.5f;     //���뵭��ʱ��

    private Color color;

    private const string MAIN_MENU = "MainMenu";
    private const string GAMEPLAY = "Gameplay";

    /// <summary>
    /// ���س���
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <returns></returns>
    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);  //�����첽����
        loadingOperation.allowSceneActivation = false;      //����������

        transitionImage.gameObject.SetActive(true);

        //��������
        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);   //����ͼƬ͸���ȣ����Ʒ�Χ[0, 1]
            transitionImage.color = color;

            yield return null;
        }

        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);    //�ȴ�ֱ����������90%����

        loadingOperation.allowSceneActivation = true;   //�����

        //��������
        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);   //����ͼƬ͸���ȣ����Ʒ�Χ[0, 1]
            transitionImage.color = color;

            yield return null;
        }

        transitionImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����Gameplay����
    /// </summary>
    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAMEPLAY));    //����Gameplay����
    }

    /// <summary>
    /// �������˵�����
    /// </summary>
    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));    //�������˵�����
    }
}
