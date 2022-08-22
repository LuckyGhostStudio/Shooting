using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas;

    [Header("Button")]

    [SerializeField] private Button startButton;        //��ʼ
    [SerializeField] private Button optionsButton;      //ѡ��
    [SerializeField] private Button quitButton;         //�˳�

    private void OnEnable()
    {
        //��Ӱ�ť����ť�����ֵ�
        ButtonPressedBehavior.buttonFunctionTable.Add(startButton.gameObject.name, OnStartButtonClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(quitButton.gameObject.name, OnQuitButtonClicked);
    }

    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    private void Start()
    {
        Time.timeScale = 1;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(startButton);     //Ĭ��ѡ��Start��ť
    }

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    private void OnStartButtonClicked()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplayScene();   //����Gameplay����
    }

    /// <summary>
    /// ѡ��
    /// </summary>
    private void OnOptionsButtonClicked()
    {
        UIInput.Instance.SelectUI(optionsButton);
    }

    /// <summary>
    /// �˳�
    /// </summary>
    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    //�˳��༭������
#else
        Application.Quit();
#endif
    }
}
