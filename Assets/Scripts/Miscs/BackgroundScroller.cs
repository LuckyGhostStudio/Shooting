using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Vector2 scrollVelocity;    //滚动速度
    private Material material;  //背景材质

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;     //改变材质mainTexture偏移
    }
}
