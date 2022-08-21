using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] private PlayerInput playerInput;

    private InputSystemUIInputModule UIInputModule;     //UI�������ģ��

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    /// <summary>
    /// ѡ��UI����
    /// </summary>
    /// <param name="UIObject">Ҫѡ�еĶ���</param>
    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    /// <summary>
    /// ��������UI����
    /// </summary>
    public void DisableAllUIInput()
    {
        //playerInput.DisableAllInput();  //������������
        UIInputModule.enabled = false;  //����UIģ��
    }
}
