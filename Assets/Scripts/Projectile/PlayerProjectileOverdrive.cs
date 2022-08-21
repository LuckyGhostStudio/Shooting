using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] private ProjectGuidenceSystem guidenceSystem;  //�Ƶ�ϵͳ

    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);   //������ù���Ŀ��
        transform.rotation = Quaternion.identity;   //�����ӵ��Ƕ�

        if (target == null)
        {
            base.OnEnable();    //�ӵ���ǰ�ƶ�
        }
        else
        {
            //׷��Ŀ��
            StartCoroutine(guidenceSystem.HomingCoroutine(target));
        }
    }
}
