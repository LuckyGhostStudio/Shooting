using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Death")]

    [SerializeField] private GameObject deathVFX;       //����Ч��
    [SerializeField] private AudioData[] deathSFX;      //������ը��Ч

    [Header("Health")]

    [SerializeField] protected int maxHealth;

    [SerializeField] private bool showOnHeadHealthBar = true;   //�Ƿ���ʾѪ��

    [SerializeField] private StateBar onHeadHealthBar;          //Ѫ��

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
    /// ��ʾѪ��
    /// </summary>
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);      //��ʼ��Ѫ��
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    public void HideOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }

    /// <summary>
    /// �ܵ��˺�
    /// </summary>
    /// <param name="damage">�˺�ֵ</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (showOnHeadHealthBar && gameObject.activeSelf)
        {
            onHeadHealthBar.UpdateState(health, maxHealth); //����Ѫ��
        }

        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// ��ɫ����
    /// </summary>
    public virtual void Die()
    {
        health = 0;
        AudioManager.Instance.PlayRandomSFX(deathSFX);      //���ű�ը��Ч
        PoolManager.Release(deathVFX, transform.position);  //��������Ч��
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �ظ�����ֵ
    /// </summary>
    /// <param name="value">ֵ</param>
    public virtual void RestoreHealth(int value)
    {
        if (health == maxHealth) return;

        health = Mathf.Clamp(health + value, 0, maxHealth); //����ֵ����value���������� [0, maxHealth]

        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdateState(health, maxHealth); //����Ѫ��
        }
    }

    /// <summary>
    /// �����ظ�����ֵ
    /// </summary>
    /// <param name="waitTime">ʱ����</param>
    /// <param name="percent">�ظ��ٷֱ�</param>
    /// <returns></returns>
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health < maxHealth)
        {
            yield return waitTime;
            RestoreHealth((int)(maxHealth * percent));  //�ظ�����ֵ
        }
    }

    /// <summary>
    /// �������˺�
    /// </summary>
    /// <param name="waitTime">ʱ����</param>
    /// <param name="percent">���˰ٷֱ�</param>
    /// <returns></returns>
    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while (health > 0)
        {
            yield return waitTime;
            TakeDamage((int)(maxHealth * percent));  //��������ֵ
        }
    }
}
