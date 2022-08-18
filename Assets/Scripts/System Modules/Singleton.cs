using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component //类型约束
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;   //将this转换为T
    }
}
