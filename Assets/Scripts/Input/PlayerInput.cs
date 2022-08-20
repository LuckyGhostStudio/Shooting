using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
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

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);   //登记Gamplay动作表回调函数
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    /// <summary>
    /// 禁用所有输入
    /// </summary>
    public void DisableAllInput()
    {
        inputActions.Gameplay.Disable();    //禁用Gameplay输入
    }

    /// <summary>
    /// 启用Gameplay输入
    /// </summary>
    public void EnableGameplayInput()
    {
        inputActions.Gameplay.Enable();     //启用Gameplay输入

        Cursor.visible = false;                     //隐藏鼠标指针
        Cursor.lockState = CursorLockMode.Locked;   //锁定鼠标指针
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)    //输入动作执行时，相当于GetKey
        {
            onMove.Invoke(context.ReadValue<Vector2>());
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
}
