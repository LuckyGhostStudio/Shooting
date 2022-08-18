using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab { get { return prefab; } }

    public int Size { get { return size; } }    //����ش�С
    public int RuntimeSize { get { return queue.Count; } }  //���������ʱ��С

    [SerializeField] private GameObject prefab;

    [SerializeField] private int size = 1;

    Queue<GameObject> queue;

    Transform parent;   //���Ƶ��¶���ĸ���

    /// <summary>
    /// ��ʼ�������
    /// </summary>
    /// <param name="parent">����ĸ���</param>
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());  //Ԥ�ƶ����������
        }
    }

    /// <summary>
    /// ����Ԥ����
    /// </summary>
    /// <returns></returns>
    private GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);
        copy.SetActive(false);
        return copy;
    }

    /// <summary>
    /// ��ÿ��ö���
    /// </summary>
    /// <returns></returns>
    private GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)    //�������ж��� ��ͷԪ��δ����
        {
            availableObject = queue.Dequeue();  //��ͷ�������
        }
        else
        {
            availableObject = Copy();   //����һ������
        }

        queue.Enqueue(availableObject); //��ӣ���ǰ��ӣ�ʡȥʹ��������ӵĲ�����

        return availableObject;
    }

    /// <summary>
    /// ���ÿ��ö���
    /// </summary>
    /// <returns></returns>
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();  //��ÿ��ö���

        preparedObject.SetActive(true);     //���ö���

        return preparedObject;
    }

    /// <summary>
    /// ���ÿ��ö���
    /// </summary>
    /// <param name="position">λ��</param>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();  //��ÿ��ö���

        preparedObject.SetActive(true);     //���ö���
        preparedObject.transform.position = position;

        return preparedObject;
    }

    /// <summary>
    /// ���ÿ��ö���
    /// </summary>
    /// <param name="position">λ��</param>
    /// <param name="rotation">��ת</param>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();  //��ÿ��ö���

        preparedObject.SetActive(true);     //���ö���
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    /// <summary>
    /// ���ÿ��ö���
    /// </summary>
    /// <param name="position">λ��</param>
    /// <param name="rotation">��ת</param>
    /// <param name="localScale">����</param>
    /// <returns></returns>
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();  //��ÿ��ö���

        preparedObject.SetActive(true);     //���ö���
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }

    ///// <summary>
    ///// ��ʹ����Ķ��󷵻ض����
    ///// </summary>
    ///// <param name="gameObject"></param>
    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);  //���
    //}
}
