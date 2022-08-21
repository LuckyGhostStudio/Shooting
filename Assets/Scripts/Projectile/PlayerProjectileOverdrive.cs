using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] private ProjectGuidenceSystem guidenceSystem;  //制导系统

    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);   //随机设置攻击目标
        transform.rotation = Quaternion.identity;   //重置子弹角度

        if (target == null)
        {
            base.OnEnable();    //子弹向前移动
        }
        else
        {
            //追踪目标
            StartCoroutine(guidenceSystem.HomingCoroutine(target));
        }
    }
}
