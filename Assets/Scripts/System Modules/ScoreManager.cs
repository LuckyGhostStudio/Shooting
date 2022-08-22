using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    public int Score => score;

    private int score;
    private int currentScore;

    private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);   //�����ı�����ֵ

    #region Score Display

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
    #endregion

    #region High Scoring System

    /// <summary>
    /// ��ҵ÷�
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
    /// ��ҷ�������
    /// </summary>
    [System.Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> scoreList = new List<PlayerScore>();   //�����б�
    }

    readonly string saveFileName = "player_score.json";     //Player��������json�ļ�
    private string playerName = "None";

    public bool HasNewHighScore => score > LoadPlayerScoreData().scoreList[9].score;    //�÷��Ƿ����ǰ10

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    /// <summary>
    /// ����Player�÷�����
    /// </summary>
    public void SavePlayerScoreData()
    {
        PlayerScoreData playerScoreData = LoadPlayerScoreData();                //��ȡ�浵������
        
        playerScoreData.scoreList.Add(new PlayerScore(score, playerName));      //���������ݵ��б�
        playerScoreData.scoreList.Sort((x, y) => y.score.CompareTo(x.score));   //������ݰ��÷ֽ�������

        SaveSystem.Save(saveFileName, playerScoreData);      //��������
    }

    /// <summary>
    /// ����Player��������
    /// </summary>
    /// <returns>�÷�����</returns>
    public PlayerScoreData LoadPlayerScoreData()
    {
        PlayerScoreData playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(saveFileName))    //�ļ�����
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(saveFileName);   //��ȡ���������
        }
        else
        {
            //����Ĭ�ϴ浵�����10���������ݵ��б�
            while (playerScoreData.scoreList.Count < 10) 
            { 
                playerScoreData.scoreList.Add(new PlayerScore(0, playerName));  //���Ĭ�����ݵ��б�
            }

            SaveSystem.Save(saveFileName, playerScoreData);     //��������
        }

        return playerScoreData;
    }

    #endregion
}
