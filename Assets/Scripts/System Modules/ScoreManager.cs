using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    private int score;
    private int currentScore;

    private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);   //�����ı�����ֵ

    /// <summary>
    /// ���÷���
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateScoreText(score);    //���·���UI
    }

    /// <summary>
    /// ���ӵ÷�
    /// </summary>
    /// <param name="scorePoint">�÷ֵ�</param>
    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));  //���ӵ÷�
    }

    /// <summary>
    /// ��̬���ӵ÷�
    /// </summary>
    /// <returns></returns>
    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);     //���ŷ����ı�

        //�������ӵ�ʵ�ʵ÷�
        while (score < currentScore)
        {
            score++;    //һ��һ������
            ScoreDisplay.UpdateScoreText(score);    //���·���UI

            yield return null;
        }

        ScoreDisplay.ScaleText(Vector3.one);    //�����ı����Ż�ԭ
    }
}
