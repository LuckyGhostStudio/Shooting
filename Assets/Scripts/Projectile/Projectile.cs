using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitVFX;     //命中效果
    [SerializeField] private AudioData[] hitSFX;      //命中音效

    [SerializeField] private int damage;
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected Vector2 moveDirection;

    protected GameObject target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectlyCoroutine());     //子弹移动
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
    /// 移动
    /// </summary>
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Character>(out Character character))    //碰撞到Character类对象
        {
            character.TakeDamage(damage);

            //从对象池取出hitVFX对象 旋转值：碰撞点法线方向
            PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(hitSFX);    //播放命中音效

            gameObject.SetActive(false);    //禁用子弹对象
        }
    }

    /// <summary>
    /// 设置目标对象
    /// </summary>
    /// <param name="target">目标</param>
    protected void SetTarget(GameObject target) => this.target = target;
}
