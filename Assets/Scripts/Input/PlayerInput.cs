using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions, InputActions.IPauseMenuActions
{
    //�ƶ��¼�
    public event UnityAction<Vector2> onMove = delegate { };
    public event UnityAction onStopMove = delegate { };

    //�����¼�
    public event UnityAction onFire = delegate { };
    public event UnityAction onStopFire = delegate { };

    //�����¼�
    public event UnityAction onDodge = delegate { };

    //�����¼�����������
    public event UnityAction onOverdrive = delegate { };

    //���䵼���¼�
    public event UnityAction onLaunchMissile = delegate { };

    //��ͣ�¼�
    public event UnityAction onPause = delegate { };

    //ȡ����ͣ�¼�
    public event UnityAction onUnpause = delegate { };

    InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);   //�Ǽ�Gamplay������ص�����
        inputActions.PauseMenu.SetCallbacks(this);  //�Ǽ�PauseMenu������ص�����
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    /// <summary>
    /// �л�������
    /// </summary>
    /// <param name="actionMap">��Ҫ�л����Ķ�����</param>
    /// /// <param name="isUIInput">�Ƿ���UI����</param>
    private void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();     //�������ж�����
        actionMap.Enable();     //����Ŀ�궯����

        if (isUIInput)
        {
            Cursor.visible = true;                      //�������ָ��
            Cursor.lockState = CursorLockMode.None;     //ȡ���������ָ��
        }
        else
        {
            Cursor.visible = false;                     //�������ָ��
            Cursor.lockState = CursorLockMode.Locked;   //�������ָ��
        }
    }

    /// <summary>
    /// �л�����̬����ģʽ�����²���timeScaleӰ��
    /// </summary>
    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;

    /// <summary>
    /// �л����̶�֡����ģʽ
    /// </summary>
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    /// <summary>
    /// ������������
    /// </summary>
    public void DisableAllInput() => inputActions.Disable();     //�������ж�����

    /// <summary>
    /// �л���Gameplay����
    /// </summary>
    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);  //�л���Gameplay������

    /// <summary>
    /// �л���PauseMenu����
    /// </summary>
    public void EnablePauseMenuInput() => SwitchActionMap(inputActions.PauseMenu, true);  //�л���PauseMenu������

    /// <summary>
    /// �ƶ�
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)    //���붯��ִ��ʱ���൱��GetKey
        {
            onMove.Invoke(context.ReadValue<Vector2>());    //ִ���¼�
        }
        if(context.canceled)    //���붯��ȡ��ʱ
        {
            onStopMove.Invoke();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)    //���붯��ִ��ʱ���൱��GetKey
        {
            onFire.Invoke();
        }
        if (context.canceled)    //���붯��ȡ��ʱ
        {
            onStopFire.Invoke();
        }
    }

    /// <summary>
    /// ����
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
    /// ���٣���������
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
    /// ��ͣ��Ϸ
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
    /// ȡ����ͣ
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
    /// ���䵼��
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
