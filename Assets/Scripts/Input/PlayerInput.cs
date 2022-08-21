using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions, InputActions.IPauseMenuActions
{
    //移动事件
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };

    //开火事件
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    //闪避事件
    public event UnityAction onDodge = delegate { };

    //过速事件：能量爆发
    public event UnityAction onOverdrive = delegate { };

    //发射导弹事件
    public event UnityAction onLaunchMissile = delegate { };

    //暂停事件
    public event UnityAction onPause = delegate { };

    //取消暂停事件
    public event UnityAction onUnpause = delegate { };

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);   //登记Gamplay动作表回调函数
        inputActions.PauseMenu.SetCallbacks(this);  //登记PauseMenu动作表回调函数
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    /// <summary>
    /// 切换动作表
    /// </summary>
    /// <param name="actionMap">需要切换到的动作表</param>
    /// /// <param name="isUIInput">是否是UI输入</param>
    private void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();     //禁用所有动作表
        actionMap.Enable();     //启用目标动作表

        if (isUIInput)
        {
            Cursor.visible = true;                      //启用鼠标指针
            Cursor.lockState = CursorLockMode.None;     //取消锁定鼠标指针
        }
        else
        {
            Cursor.visible = false;                     //隐藏鼠标指针
            Cursor.lockState = CursorLockMode.Locked;   //锁定鼠标指针
        }
    }

    /// <summary>
    /// 切换到动态更新模式：更新不受timeScale影响
    /// </summary>
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    /// <summary>
    /// 切换到固定帧更新模式
    /// </summary>
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    /// <summary>
    /// 禁用所有输入
    /// </summary>
    public void DisableAllInput() => inputActions.Disable();     //禁用所有动作表

    /// <summary>
    /// 切换到Gameplay输入
    /// </summary>
    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);  //切换到Gameplay动作表

    /// <summary>
    /// 切换到PauseMenu输入
    /// </summary>
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);  //切换到PauseMenu动作表

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)    //输入动作执行时，相当于GetKey
        {
            onMove.Invoke(context.ReadValue<Vector2>());    //执行事件
        }
        if(context.canceled)    //输入动作取消时
        {
            onStopMove.Invoke();
        }
    }

    /// <summary>
    /// 开火
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)    //输入动作执行时，相当于GetKey
        {
            onFire.Invoke();
        }
        if (context.canceled)    //输入动作取消时
        {
            onStopFire.Invoke();
        }
    }

    /// <summary>
    /// 闪避
    /// </summary>
    /// <param name="context"></param>
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onDodge.Invoke();
        }
    }

    /// <summary>
    /// 过速：能量爆发
    /// </summary>
    /// <param name="context"></param>
    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onPause.Invoke();
        }
    }

    /// <summary>
    /// 取消暂停
    /// </summary>
    /// <param name="context"></param>
    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onUnpause.Invoke();
        }
    }

    /// <summary>
    /// 发射导弹
    /// </summary>
    /// <param name="context"></param>
    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }
}
