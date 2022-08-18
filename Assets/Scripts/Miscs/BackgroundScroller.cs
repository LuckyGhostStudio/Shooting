using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollVelocity;    //�����ٶ�
    private Material material;  //��������

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;     //�ı����mainTextureƫ��
    }
}
