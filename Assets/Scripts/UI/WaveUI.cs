using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private Text waveText;  //波数文本
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        waveText = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        waveText.text = "- WAVE " + EnemyManager.Instance.WaveNumber.ToString("00") + " -";   //显示波数
    }
}
