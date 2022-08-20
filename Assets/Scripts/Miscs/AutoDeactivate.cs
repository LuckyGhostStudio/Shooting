using System.Collections;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] bool destroyGameObject;    //�Ƿ����ٶ���
    [SerializeField] float lifetime = 3f;       //��������

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
    /// ���ٶ���
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