using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAiming : Projectile
{
    private void Awake()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player"));    //���ù���Ŀ��
    }

    protected override void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectionCoroutine));     //��׼
        base.OnEnable();
    }

    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;

        if (target.activeSelf)
        {
            moveDirection = (target.transform.position - transform.position).normalized;     //�ƶ����� ָ�� Ŀ��
        }
    }
}
