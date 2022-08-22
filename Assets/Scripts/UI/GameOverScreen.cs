using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private AudioData onConfirmGameOverSFX;    //确认GameOver音效

    private Canvas canvas;      //GameOverScreen画布
    private Animator animator;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");  //GameOver退出动画ID

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;       //进入GameOver事件
        playerInput.onConfirmGameOver += OnConfirmGameOver;     //确认GameOver事件
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        playerInput.onConfirmGameOver -= OnConfirmGameOver;
    }

    /// <summary>
    /// 进入GameOver时
    /// </summary>
    private void OnGameOver()
    {
        HUDCanvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        playerInput.DisableAllInput();      //禁用所有输入
    }

    /// <summary>
    /// 确认GameOver时
    /// </summary>
    private void OnConfirmGameOver()
    {
        AudioManager.Instance.PlayRandomSFX(onConfirmGameOverSFX);  //播放确认游戏结束音效
        playerInput.DisableAllInput();              //禁用所有输入
        animator.Play(exitStateID);                 //播放GameOver退出动画
        SceneLoader.Instance.LoadScoringScene();    //加载计分场景
    }

    /// <summary>
    /// 切换到GameOverScreen动作表：GameOverEnter动画播放结束时调用（动画事件）
    /// </summary>
    private void EnableGameOverScreenInput()
    {
        playerInput.EnableGameOverScreenInput();    //切换到GameOverScreen动作表
    }
}
