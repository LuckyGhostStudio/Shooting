using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private static Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void Start()
    {
        ScoreManager.Instance.ResetScore(); //重置分数
    }

    /// <summary>
    /// 更新分数文本
    /// </summary>
    /// <param name="score">分数</param>
    public static void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// 缩放分数文本
    /// </summary>
    /// <param name="targetScale">目标值</param>
    public static void ScaleText(Vector3 targetScale)
    {
        scoreText.rectTransform.localScale = targetScale;
    }
}
