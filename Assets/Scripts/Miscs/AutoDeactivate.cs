using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{

    [SerializeField] private bool destoryGameObject;    //�Ƿ����ٴ˶���
    [SerializeField] private float lifeTime = 3f;       //��������

    WaitForSeconds waitLifeTime;

    void Awake()
    {
        waitLifeTime = new WaitForSeconds(lifeTime);
    }

    void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }

    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifeTime;

        if (destoryGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
