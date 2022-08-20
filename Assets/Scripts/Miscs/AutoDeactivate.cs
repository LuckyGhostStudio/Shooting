using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] bool destroyGameObject;    //是否销毁对象
    [SerializeField] float lifetime = 3f;       //声明周期

    WaitForSeconds waitLifetime;        

    void Awake()
    {
        waitLifetime = new WaitForSeconds(lifetime);
    }

    void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    /// <summary>
    /// 销毁对象
    /// </summary>
    /// <returns></returns>
    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifetime;

        if (destroyGameObject)
        {
            Destroy(gameObject);
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }
}