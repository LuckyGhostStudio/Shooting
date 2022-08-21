using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitVFX;     //����Ч��
    [SerializeField] private AudioData[] hitSFX;      //������Ч

    [SerializeField] private int damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectlyCoroutine());     //�ӵ��ƶ�
    }

    IEnumerator MoveDirectlyCoroutine()
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }

    /// <summary>
    /// �ƶ�
    /// </summary>
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))    //��ײ��Character�����
        {
            character.TakeDamage(damage);

            //�Ӷ����ȡ��hitVFX���� ��תֵ����ײ�㷨�߷���
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);    //����������Ч

            gameObject.SetActive(false);    //�����ӵ�����
        }
    }

    /// <summary>
    /// ����Ŀ�����
    /// </summary>
    /// <param name="target">Ŀ��</param>
    protected void SetTarget(GameObject target) => this.target = target;
}
