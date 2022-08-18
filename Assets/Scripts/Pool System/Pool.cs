using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab { get { return prefab; } }

    public int Size { get { return size; } }    //对象池大小
    public int RuntimeSize { get { return queue.Count; } }  //对象池运行时大小

    [SerializeField] private GameObject prefab;

    [SerializeField] private int size = 1;

    Queue<GameObject> queue;

    Transform parent;   //复制的新对象的父级

    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="parent">对象的父级</param>
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());  //预制对象复制体入队
        }
    }

    /// <summary>
    /// 复制预制体
    /// </summary>
    /// <returns></returns>
    private GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);
        copy.SetActive(false);
        return copy;
    }

    /// <summary>
    /// 获得可用对象
    /// </summary>
    /// <returns></returns>
    private GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)    //队列中有对象 队头元素未启用
        {
            availableObject = queue.Dequeue();  //队头对象出队
        }
        else
        {
            availableObject = Copy();   //复制一个对象
        }

        queue.Enqueue(availableObject); //入队（提前入队，省去使用完再入队的操作）

        return availableObject;
    }

    /// <summary>
    /// 启用可用对象
    /// </summary>
    /// <returns></returns>
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();  //获得可用对象

        preparedObject.SetActive(true);     //启用对象

        return preparedObject;
    }

    /// <summary>
    /// 启用可用对象
    /// </summary>
    /// <param name="position">位置</param>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();  //获得可用对象

        preparedObject.SetActive(true);     //启用对象
        preparedObject.transform.position = position;

        return preparedObject;
    }

    /// <summary>
    /// 启用可用对象
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="rotation">旋转</param>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();  //获得可用对象

        preparedObject.SetActive(true);     //启用对象
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    /// <summary>
    /// 启用可用对象
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="rotation">旋转</param>
    /// <param name="localScale">缩放</param>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();  //获得可用对象

        preparedObject.SetActive(true);     //启用对象
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }

    ///// <summary>
    ///// 将使用完的对象返回对象池
    ///// </summary>
    ///// <param name="gameObject"></param>
    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);  //入队
    //}
}
