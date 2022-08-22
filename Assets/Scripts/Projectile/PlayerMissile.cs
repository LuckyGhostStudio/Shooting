using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] private AudioData targetAcquiredVoice;     //Ŀ��������Ч 

    [SerializeField] private float lowSpeed = 8f;
    [SerializeField] private float highSpeed = 25f;
    [SerializeField] private float variableSpeedDelay = 0.5f;   //�����ӳ�ʱ��

    [SerializeField] private GameObject explosionVFX;       //��ըЧ��
    [SerializeField] private AudioData explosionSFX;        //��ը��Ч
    [SerializeField] private LayerMask enemyLayer = default;          //���˲�
    [SerializeField] private float explosionRadius = 3f;    //��ը�뾶
    [SerializeField] private int explosionDamage = 100;     //��ը�˺�

    WaitForSeconds waitForVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        waitForVariableSpeedDelay = new WaitForSeconds(variableSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        PoolManager.Release(explosionVFX, transform.position);  //���ɱ�ըЧ��
        AudioManager.Instance.PlayRandomSFX(explosionSFX);      //���ű�ը��Ч

        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);    //��ñ�ը��Χ�����е�����ײ��

        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);      //�����ܵ���ը�˺�
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);     //���Ʊ�ը��Χ
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;

        yield return waitForVariableSpeedDelay;

        moveSpeed = highSpeed;

        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquiredVoice);   //����Ŀ��������Ч
        }
    }
}
