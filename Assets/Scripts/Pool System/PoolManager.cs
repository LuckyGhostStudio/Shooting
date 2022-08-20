using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] enemyPools;     //Enemy对象池数组
    [SerializeField] private Pool[] playerProjectilePools;  //Player子弹池数组
    [SerializeField] private Pool[] enemyProjectilePools;   //Enemy子弹池数组
    [SerializeField] private Pool[] vFXPools;   //VFX池数组

    private static Dictionary<GameObject, Pool> dictionary;    //预制体和对象池的对应字典

    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();

        Initialize(enemyPools);     //初始化Enemy对象池数组
        Initialize(playerProjectilePools);  //初始化Player子弹池数组
        Initialize(enemyProjectilePools);   //初始化Enemy子弹池数组
        Initialize(vFXPools);   //初始化VFX池数组
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
    /// 检查对象池容量
    /// </summary>
    /// <param name="pools"></param>
    void CheckPoolSize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning("Pool:" + pool.Prefab.name + "运行时容量" + pool.RuntimeSize + "大于初始化容量" + pool.Size);
            }
        }
    }

    /// <summary>
    /// 初始化对象池数组
    /// </summary>
    /// <param name="pools"></param>
    void Initialize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
#if UNITY_EDITOR    //在unity编辑器中才编译运行
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("对象池添加了相同的预制体：" + pool.Prefab.name);
                continue;  //键重复，跳过
            }
#endif

            dictionary.Add(pool.Prefab, pool);  //pool添加到字典

            Transform poolParent = new GameObject("Pool：" + pool.Prefab.name).transform;    //Player子弹池的父级
            poolParent.parent = transform;  //池的父级为Pool Manager的子级

            pool.Initialize(poolParent);    //初始化对象池
        }
    }

    /// <summary>
    /// 根据传入prefab查询字典，返回对象池中预备好的对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR    //在unity编辑器中才编译运行
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池预制体不存在：" + prefab.name);
            return null;  
        }
#endif
        return dictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// 根据传入prefab查询字典，返回对象池中预备好的对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR    //在unity编辑器中才编译运行
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池预制体不存在：" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// 根据传入prefab查询字典，返回对象池中预备好的对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR    //在unity编辑器中才编译运行
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池预制体不存在：" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation) ;
    }

    /// <summary>
    /// 根据传入prefab查询字典，返回对象池中预备好的对象
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR    //在unity编辑器中才编译运行
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("对象池预制体不存在：" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
