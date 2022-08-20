using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    private int score;
    private int currentScore;

    private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);   //分数文本缩放值

    /// <summary>
    /// 重置分数
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateScoreText(score);    //更新分数UI
    }

    /// <summary>
    /// 增加得分
    /// </summary>
    /// <param name="scorePoint">得分点</param>
    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));  //增加得分
    }

    /// <summary>
    /// 动态增加得分
    /// </summary>
    /// <returns></returns>
    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);     //缩放分数文本

        //分数增加到实际得分
        while (score < currentScore)
        {
            score++;    //一分一分增加
            ScoreDisplay.UpdateScoreText(score);    //更新分数UI

            yield return null;
        }

        ScoreDisplay.ScaleText(Vector3.one);    //分数文本缩放还原
    }
}
