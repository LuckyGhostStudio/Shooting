using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Canvas mainMenuCanvas;

    [Header("Button")]

    [SerializeField] private Button startButton;        //开始
    [SerializeField] private Button optionsButton;      //选项
    [SerializeField] private Button quitButton;         //退出

    private void OnEnable()
    {
        //添加按钮到按钮功能字典
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
        UIInput.Instance.SelectUI(startButton);     //默认选中Start按钮
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void OnStartButtonClicked()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplayScene();   //加载Gameplay场景
    }

    /// <summary>
    /// 选项
    /// </summary>
    private void OnOptionsButtonClicked()
    {
        UIInput.Instance.SelectUI(optionsButton);
    }

    /// <summary>
    /// 退出
    /// </summary>
    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    //退出编辑器运行
#else
        Application.Quit();
#endif
    }
}
