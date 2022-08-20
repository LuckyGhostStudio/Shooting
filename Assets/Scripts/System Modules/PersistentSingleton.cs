using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else if (Instance != this)  //不是当前对象 存在其他实例
        {
            Destroy(gameObject);    //销毁
        }

        DontDestroyOnLoad(gameObject);  //加载场景时不销毁此对象
    }
}
