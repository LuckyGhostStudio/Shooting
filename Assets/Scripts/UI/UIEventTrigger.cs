using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, ISelectHandler, ISubmitHandler
{
    [SerializeField] private AudioData selectSFX;   //ѡ��ʱ��Ч
    [SerializeField] private AudioData submitSFX;   //�ύʱ��Ч

    /// <summary>
    /// ����ڶ����ϰ���ʱ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }

    /// <summary>
    /// �����ͣ�ڶ�����ʱ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    /// <summary>
    /// ѡ�ж���ʱ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(selectSFX);
    }

    /// <summary>
    /// �ύʱ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySFX(submitSFX);
    }
}
