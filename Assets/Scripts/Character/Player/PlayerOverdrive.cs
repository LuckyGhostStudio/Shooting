using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };    //�����¼�
    public static UnityAction off = delegate { };   //�ر��¼�

    [SerializeField] GameObject triggerVFX;             //������������Ч��
    [SerializeField] GameObject engineVFXNormal;        //��������Ч��
    [SerializeField] GameObject engineVFXOverdrive;     //������������Ч��

    [SerializeField] AudioData onSFX;       //����ʱ��Ч
    [SerializeField] AudioData offSFX;      //�ر�ʱ��Ч

    private void Awake()
    {
        on += On;
        off += Off;
    }

    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }

    /// <summary>
    /// ����
    /// </summary>
    private void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    /// <summary>
    /// �ر�
    /// </summary>
    private void Off()
    {
        engineVFXNormal.SetActive(true);
        engineVFXOverdrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
