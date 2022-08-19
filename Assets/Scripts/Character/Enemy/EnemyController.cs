using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move")]

    [SerializeField] private Vector2 padding;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRotationAngle = 25f;

    [Header("Fire")]

    [SerializeField] private float minFireInterval;
    [SerializeField] private float maxFireInterval;
    [SerializeField] private GameObject[] projectiles;  //子弹数组
    [SerializeField] private Transform muzzle;

    void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));    //移动
        StartCoroutine(nameof(RandomlyFireCoroutine));      //开火
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 随机移动
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(padding.x, padding.y);      //出生位置
        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(padding.x, padding.y);   //目标位置

        while (gameObject.activeSelf)   //对象启用时
        {
            if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)   //当前位置不在目标位置
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);   //移动到目标位置
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);   //移动时沿x轴旋转
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(padding.x, padding.y);   //随机生成目标位置
            }

            yield return null;  //不等待，直到下一帧执行
        }
    }

    /// <summary>
    /// 随机开火
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));    //等待随机开火间隔

            foreach (GameObject projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);   //根据子弹prefab从对象池取出子弹并启用
            }
        }
    }
}
