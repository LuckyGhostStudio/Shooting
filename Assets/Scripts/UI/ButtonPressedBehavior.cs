using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPressedBehavior : StateMachineBehaviour
{
    public static Dictionary<string, UnityAction> buttonFunctionTable;  //按钮功能表

    private void Awake()
    {
        buttonFunctionTable = new Dictionary<string, UnityAction>();
    }

    /// <summary>
    /// 开始播放动画时调用
    /// </summary>
    /// <param name="animator">当前动画控制器</param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIInput.Instance.DisableAllUIInput();   //禁用所有UI输入
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        buttonFunctionTable[animator.gameObject.name].Invoke();     //调用播放动画的按钮的功能函数：animator为此脚本挂载的Animator
        //UIInput.Instance.DisableAllUIInput();   //禁用所有UI输入
    }
}
