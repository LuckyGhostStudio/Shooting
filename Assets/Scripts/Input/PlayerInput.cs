using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
{
    //�ƶ��¼�
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };

    //�����¼�
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);   //�Ǽ�Gamplay������ص�����
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    /// <summary>
    /// ������������
    /// </summary>
    public void DisableAllInput()
    {
        inputActions.Gameplay.Disable();    //����Gameplay����
    }

    /// <summary>
    /// ����Gameplay����
    /// </summary>
    public void EnableGameplayInput()
    {
        inputActions.Gameplay.Enable();     //����Gameplay����

        Cursor.visible = false;                     //�������ָ��
        Cursor.lockState = CursorLockMode.Locked;   //�������ָ��
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)    //���붯��ִ��ʱ���൱��GetKey
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.phase==InputActionPhase.Canceled)    //���붯��ȡ��ʱ
        {
            onStopMove.Invoke();
        }
    }

    public void OnFIre(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)    //���붯��ִ��ʱ���൱��GetKey
        {
            onFire.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)    //���붯��ȡ��ʱ
        {
            onStopFire.Invoke();
        }
    }
}
