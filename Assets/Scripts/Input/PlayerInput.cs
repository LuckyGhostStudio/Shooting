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

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)    //输入动作执行时，相当于GetKey
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.phase==InputActionPhase.Canceled)    //输入动作取消时
        {
            onStopMove.Invoke();
        }
    }

    public void OnFIre(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)    //输入动作执行时，相当于GetKey
        {
            onFire.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)    //输入动作取消时
        {
            onStopFire.Invoke();
        }
    }
}
