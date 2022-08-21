using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private GameObject missile;    //导弹预制体
    [SerializeField] private AudioData launchSFX;   //发射音效

    /// <summary>
    /// 发射导弹
    /// </summary>
    /// <param name="muzzleTransform">枪口位置</param>
    public void Launch(Transform muzzleTransform)
    {
        PoolManager.Release(missile, muzzleTransform.position);     //从对象池启用导弹
        AudioManager.Instance.PlayRandomSFX(launchSFX);      //播放发射音效
    }
}
