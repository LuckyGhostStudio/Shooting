using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private GameObject missile;    //����Ԥ����
    [SerializeField] private AudioData launchSFX;   //������Ч

    /// <summary>
    /// ���䵼��
    /// </summary>
    /// <param name="muzzleTransform">ǹ��λ��</param>
    public void Launch(Transform muzzleTransform)
    {
        PoolManager.Release(missile, muzzleTransform.position);     //�Ӷ�������õ���
        AudioManager.Instance.PlayRandomSFX(launchSFX);      //���ŷ�����Ч
    }
}
