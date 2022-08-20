using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move")]

    [SerializeField] private Vector2 padding;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRotationAngle = 25f;     //�ƶ���ת�Ƕ�

    [Header("Fire")]

    [SerializeField] private GameObject[] projectiles;          //�ӵ�����
    [SerializeField] private AudioData[] projectileLaunchSFX;   //�ӵ�������Ч

    [SerializeField] private float minFireInterval;
    [SerializeField] private float maxFireInterval;
    [SerializeField] private Transform muzzle;

    float maxMoveDistancePerFrame;
    WaitForFixedUpdate waitForFixedUpdate;

    private void Awake()
    {
        maxMoveDistancePerFrame = moveSpeed * Time.fixedDeltaTime;  //ÿ֡�ƶ�����
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));    //�ƶ�
        StartCoroutine(nameof(RandomlyFireCoroutine));      //����
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// ����ƶ�
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(padding.x, padding.y);      //����λ��
        Vector3 targetPosition = Viewport.Instance.RandomRightHalfPosition(padding.x, padding.y);   //Ŀ��λ��

        while (gameObject.activeSelf)   //��������ʱ
        {
            if (Vector3.Distance(transform.position, targetPosition) > maxMoveDistancePerFrame)   //��ǰλ�ò���Ŀ��λ��
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);   //�ƶ���Ŀ��λ��
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);   //�ƶ�ʱ��x����ת
            }
            else
            {
                targetPosition = Viewport.Instance.RandomRightHalfPosition(padding.x, padding.y);   //�������Ŀ��λ��
            }

            yield return waitForFixedUpdate;  //����һִ֡��
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));    //�ȴ����������

            foreach (GameObject projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);   //�����ӵ�prefab�Ӷ����ȡ���ӵ�������
            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);   //�����ӵ�������Ч
        }
    }
}
