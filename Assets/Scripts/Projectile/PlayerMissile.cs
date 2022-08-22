using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverdrive
{
    [SerializeField] private AudioData targetAcquiredVoice;     //目标锁定音效 

    [SerializeField] private float lowSpeed = 8f;
    [SerializeField] private float highSpeed = 25f;
    [SerializeField] private float variableSpeedDelay = 0.5f;   //变速延迟时间

    [SerializeField] private GameObject explosionVFX;       //爆炸效果
    [SerializeField] private AudioData explosionSFX;        //爆炸音效
    [SerializeField] private LayerMask enemyLayer = default;          //敌人层
    [SerializeField] private float explosionRadius = 3f;    //爆炸半径
    [SerializeField] private int explosionDamage = 100;     //爆炸伤害

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

        PoolManager.Release(explosionVFX, transform.position);  //生成爆炸效果
        AudioManager.Instance.PlayRandomSFX(explosionSFX);      //播放爆炸音效

        var colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);    //获得爆炸范围内所有敌人碰撞体

        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(explosionDamage);      //敌人受到爆炸伤害
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);     //绘制爆炸范围
    }

    /// <summary>
    /// 变速
    /// </summary>
    /// <returns></returns>
    IEnumerator VariableSpeedCoroutine()
    {
        moveSpeed = lowSpeed;

        yield return waitForVariableSpeedDelay;

        moveSpeed = highSpeed;

        if (target != null)
        {
            AudioManager.Instance.PlayRandomSFX(targetAcquiredVoice);   //播放目标锁定音效
        }
    }
}
