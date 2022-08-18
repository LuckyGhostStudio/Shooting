using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{

    [SerializeField] private bool destoryGameObject;    //是否销毁此对象
    [SerializeField] private float lifeTime = 3f;       //生命周期

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
