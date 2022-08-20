using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitVFX;     //����Ч��
    [SerializeField] private AudioData[] hitSFX;      //������Ч

    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());     //�ӵ��ƶ�
    }

    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

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
}
