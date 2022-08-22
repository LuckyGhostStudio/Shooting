using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler, ISubmitHandler
{
    [SerializeField] private AudioData selectSFX;   //选中时音效
    [SerializeField] private AudioData submitSFX;   //提交时音效

    /// <summary>
    /// 鼠标在对象上按下时调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }

    /// <summary>
    /// 鼠标悬停在对象上时调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    /// <summary>
    /// 选中对象时调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    /// <summary>
    /// 提交时调用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
