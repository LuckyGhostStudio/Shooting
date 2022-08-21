using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] private PlayerInput playerInput;

    private InputSystemUIInputModule UIInputModule;     //UI输入控制模块

    protected override void Awake()
    {
        base.Awake();
        UIInputModule = GetComponent<InputSystemUIInputModule>();
        UIInputModule.enabled = false;
    }

    /// <summary>
    /// 选择UI对象
    /// </summary>
    /// <param name="UIObject">要选中的对象</param>
    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        UIInputModule.enabled = true;
    }

    /// <summary>
    /// 禁用所有UI输入
    /// </summary>
    public void DisableAllUIInput()
    {
        //playerInput.DisableAllInput();  //禁用所有输入
        UIInputModule.enabled = false;  //禁用UI模块
    }
}
