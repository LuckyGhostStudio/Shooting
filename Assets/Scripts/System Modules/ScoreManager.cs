using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    public int Score => score;

    private int score;
    private int currentScore;

    private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);   //分数文本缩放值

    #region Score Display

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
    #endregion

    #region High Scoring System

    /// <summary>
    /// 玩家得分
    /// </summary>
    [System.Serializable]
    public class PlayerScore
    {
        public int score;
        public string playerName;

        public PlayerScore(int score,string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

    /// <summary>
    /// 玩家分数数据
    /// </summary>
    [System.Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> scoreList = new List<PlayerScore>();   //分数列表
    }

    readonly string saveFileName = "player_score.json";     //Player分数数据json文件
    private string playerName = "None";

    public bool HasNewHighScore => score > LoadPlayerScoreData().scoreList[9].score;    //得分是否进入前10

    /// <summary>
    /// 设置玩家名字
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    /// <summary>
    /// 保存Player得分数据
    /// </summary>
    public void SavePlayerScoreData()
    {
        PlayerScoreData playerScoreData = LoadPlayerScoreData();                //读取存档的数据
        
        playerScoreData.scoreList.Add(new PlayerScore(score, playerName));      //添加玩家数据到列表
        playerScoreData.scoreList.Sort((x, y) => y.score.CompareTo(x.score));   //玩家数据按得分降序排序

        SaveSystem.Save(saveFileName, playerScoreData);      //保存数据
    }

    /// <summary>
    /// 加载Player分数数据
    /// </summary>
    /// <returns>得分数据</returns>
    public PlayerScoreData LoadPlayerScoreData()
    {
        PlayerScoreData playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(saveFileName))    //文件存在
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(saveFileName);   //读取保存的数据
        }
        else
        {
            //创建默认存档：添加10个分数数据到列表
            while (playerScoreData.scoreList.Count < 10) 
            { 
                playerScoreData.scoreList.Add(new PlayerScore(0, playerName));  //添加默认数据到列表
            }

            SaveSystem.Save(saveFileName, playerScoreData);     //保存数据
        }

        return playerScoreData;
    }

    #endregion
}
