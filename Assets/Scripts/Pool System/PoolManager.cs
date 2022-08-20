using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] enemyPools;     //Enemy���������
    [SerializeField] private Pool[] playerProjectilePools;  //Player�ӵ�������
    [SerializeField] private Pool[] enemyProjectilePools;   //Enemy�ӵ�������
    [SerializeField] private Pool[] vFXPools;   //VFX������

    private static Dictionary<GameObject, Pool> dictionary;    //Ԥ����Ͷ���صĶ�Ӧ�ֵ�

    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();

        Initialize(enemyPools);     //��ʼ��Enemy���������
        Initialize(playerProjectilePools);  //��ʼ��Player�ӵ�������
        Initialize(enemyProjectilePools);   //��ʼ��Enemy�ӵ�������
        Initialize(vFXPools);   //��ʼ��VFX������
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
    }
#endif

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="pools"></param>
    void CheckPoolSize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning("Pool:" + pool.Prefab.name + "����ʱ����" + pool.RuntimeSize + "���ڳ�ʼ������" + pool.Size);
            }
        }
    }

    /// <summary>
    /// ��ʼ�����������
    /// </summary>
    /// <param name="pools"></param>
    void Initialize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
#if UNITY_EDITOR    //��unity�༭���вű�������
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("������������ͬ��Ԥ���壺" + pool.Prefab.name);
                continue;  //���ظ�������
            }
#endif

            dictionary.Add(pool.Prefab, pool);  //pool��ӵ��ֵ�

            Transform poolParent = new GameObject("Pool��" + pool.Prefab.name).transform;    //Player�ӵ��صĸ���
            poolParent.parent = transform;  //�صĸ���ΪPool Manager���Ӽ�

            pool.Initialize(poolParent);    //��ʼ�������
        }
    }

    /// <summary>
    /// ���ݴ���prefab��ѯ�ֵ䣬���ض������Ԥ���õĶ���
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR    //��unity�༭���вű�������
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�����Ԥ���岻���ڣ�" + prefab.name);
            return null;  
        }
#endif
        return dictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// ���ݴ���prefab��ѯ�ֵ䣬���ض������Ԥ���õĶ���
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR    //��unity�༭���вű�������
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�����Ԥ���岻���ڣ�" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// ���ݴ���prefab��ѯ�ֵ䣬���ض������Ԥ���õĶ���
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR    //��unity�༭���вű�������
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�����Ԥ���岻���ڣ�" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation) ;
    }

    /// <summary>
    /// ���ݴ���prefab��ѯ�ֵ䣬���ض������Ԥ���õĶ���
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR    //��unity�༭���вű�������
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("�����Ԥ���岻���ڣ�" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
