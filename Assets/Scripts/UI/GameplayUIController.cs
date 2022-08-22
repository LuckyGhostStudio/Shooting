using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [Header("Audio Data")]

    [SerializeField] private AudioData pauseSFX;    //暂停音效
    [SerializeField] private AudioData unpauseSFX;  //取消暂停音效

    [Header("Canvas")]

    [SerializeField] private Canvas hUDCanvas;      //HUB界面UI
    [SerializeField] private Canvas menusCanvas;    //暂停菜单UI

    [Header("Button")]

    [SerializeField] private Button resumeButton;   //取消暂停按钮
    [SerializeField] private Button optionsButton;  //选项按钮
    [SerializeField] private Button mainMenuButton; //主菜单按钮

    int buttonPressedParameterID = Animator.StringToHash("Pressed");    //Pressed的ID

    private void OnEnable()
    {
        //订阅事件
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        //将按钮名字和其功能函数添加到字典
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        //取消订阅事件
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        ButtonPressedBehavior.buttonFunctionTable.Clear();  //清空按钮功能函数字典
    }

    /// <summary>
    /// 暂停
    /// </summary>
    private void Pause()
    {
        TimeController.Instance.Pause();            //暂停
        //hUDCanvas.enabled = false;                //禁用HUD UI
        menusCanvas.enabled = true;                 //启用暂停菜单UI
        GameManager.GameState = GameState.Paused;   //暂停状态
        playerInput.EnablePauseMenuInput();         //切换到PauseMenu动作表
        playerInput.SwitchToDynamicUpdateMode();    //切换到动态更新模式
        UIInput.Instance.SelectUI(resumeButton);    //选中返回按钮
        AudioManager.Instance.PlaySFX(pauseSFX);    //播放暂停音效
    }

    /// <summary>
    /// 取消暂停
    /// </summary>
    private void Unpause()
    {
        resumeButton.Select();  //选中按钮
        resumeButton.animator.SetTrigger(buttonPressedParameterID);    //状态切换为按下
        UIInput.Instance.DisableAllUIInput();
        AudioManager.Instance.PlaySFX(unpauseSFX);    //播放取消暂停音效
    }

    /// <summary>
    /// 返回游戏
    /// </summary>
    private void OnResumeButtonClick()
    {
        TimeController.Instance.Unpause();      //取消暂停
        //hUDCanvas.enabled = true;               //启用HUD UI
        menusCanvas.enabled = false;            //禁用暂停菜单UI
        GameManager.GameState = GameState.Playing;  //运行
        playerInput.EnableGameplayInput();      //切换到Gameplay动作表
        playerInput.SwitchToFixedUpdateMode();  //切换到固定更新模式
        UIInput.Instance.SelectUI(resumeButton);    //选中返回按钮
        UIInput.Instance.DisableAllUIInput();
    }

    /// <summary>
    /// 选项
    /// </summary>
    private void OnOptionsButtonClick()
    {
        //显示选项菜单
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    /// <summary>
    /// 回到主菜单
    /// </summary>
    private void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;    //禁用暂停菜单
        SceneLoader.Instance.LoadMainMenuScene();   //加载主菜单
    }
}
