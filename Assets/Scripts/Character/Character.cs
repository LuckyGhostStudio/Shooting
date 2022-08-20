using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Death")]

    [SerializeField] private GameObject deathVFX;       //死亡效果
    [SerializeField] private AudioData[] deathSFX;      //死亡爆炸音效

    [Header("Health")]

    [SerializeField] protected int maxHealth;

    [SerializeField] private bool showOnHeadHealthBar = true;   //是否显示血条

    [SerializeField] private StateBar onHeadHealthBar;          //血条

    protected int health;

    protected virtual void OnEnable()
    {
        health = maxHealth;

        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HideOnHeadHealthBar();
        }
    }

    /// <summary>
    /// 显示血条
    /// </summary>
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);      //初始化血条
    }

    /// <summary>
    /// 隐藏血条
    /// </summary>
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (showOnHeadHealthBar && gameObject.activeSelf)
        {
            onHeadHealthBar.UpdateState(health, maxHealth); //更新血条
        }

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    public virtual void Die()
    {
        health = 0;
        AudioManager.Instance.PlayRandomSFX(deathSFX);      //播放爆炸音效
        PoolManager.Release(deathVFX, transform.position);  //生成死亡效果
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 回复生命值
    /// </summary>
    /// <param name="value">值</param>
    public virtual void RestoreHealth(int value)
    {
        if (health == maxHealth) return;

        health = Mathf.Clamp(health + value, 0, maxHealth); //生命值增加value，范限制在 [0, maxHealth]

        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health, maxHealth); //更新血条
        }
    }

    /// <summary>
    /// 持续回复生命值
    /// </summary>
    /// <param name="waitTime">时间间隔</param>
    /// <param name="percent">回复百分比</param>
    /// <returns></returns>
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth((int)(maxHealth * percent));  //回复生命值
        }
    }

    /// <summary>
    /// 持续受伤害
    /// </summary>
    /// <param name="waitTime">时间间隔</param>
    /// <param name="percent">受伤百分比</param>
    /// <returns></returns>
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0)
        {
            yield return waitTime;
            TakeDamage((int)(maxHealth * percent));  //减少生命值
        }
    }
}
