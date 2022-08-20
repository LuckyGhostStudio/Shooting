using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverdrive : MonoBehaviour
{
    public static UnityAction on = delegate { };    //开启事件
    public static UnityAction off = delegate { };   //关闭事件

    [SerializeField] GameObject triggerVFX;             //能量爆发触发效果
    [SerializeField] GameObject engineVFXNormal;        //正常引擎效果
    [SerializeField] GameObject engineVFXOverdrive;     //能量爆发引擎效果

    [SerializeField] AudioData onSFX;       //开启时音效
    [SerializeField] AudioData offSFX;      //关闭时音效

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
    /// 开启
    /// </summary>
    private void On()
    {
        triggerVFX.SetActive(true);
        engineVFXNormal.SetActive(false);
        engineVFXOverdrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    /// <summary>
    /// 关闭
    /// </summary>
    private void Off()
    {
        engineVFXNormal.SetActive(true);
        engineVFXOverdrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}
