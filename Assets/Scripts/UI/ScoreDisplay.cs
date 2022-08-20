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
        ScoreManager.Instance.ResetScore(); //���÷���
    }

    /// <summary>
    /// ���·����ı�
    /// </summary>
    /// <param name="score">����</param>
    public static void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// ���ŷ����ı�
    /// </summary>
    /// <param name="targetScale">Ŀ��ֵ</param>
    public static void ScaleText(Vector3 targetScale)
    {
        scoreText.rectTransform.localScale = targetScale;
    }
}
