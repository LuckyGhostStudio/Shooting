using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPressedBehavior : StateMachineBehaviour
{
    public static Dictionary<string, UnityAction> buttonFunctionTable;  //��ť���ܱ�

    private void Awake()
    {
        buttonFunctionTable = new Dictionary<string, UnityAction>();
    }

    /// <summary>
    /// ��ʼ���Ŷ���ʱ����
    /// </summary>
    /// <param name="animator">��ǰ����������</param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIInput.Instance.DisableAllUIInput();   //��������UI����
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        buttonFunctionTable[animator.gameObject.name].Invoke();     //���ò��Ŷ����İ�ť�Ĺ��ܺ�����animatorΪ�˽ű����ص�Animator
        //UIInput.Instance.DisableAllUIInput();   //��������UI����
    }
}
