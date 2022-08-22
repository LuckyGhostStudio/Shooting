using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [SerializeField] private Image background;  //背景
    [SerializeField] private Sprite[] backgroundImages;

    [Header("Score Screen")]

    [SerializeField] private Canvas scoringScreenCanvas;    //计分界面画布
    [SerializeField] private Text playerScoreText;          //分数文本
    [SerializeField] private Button mainMenuButton;         //主菜单按钮

    [SerializeField] private Transform highScoreLeaderBoardContainer;   //高分排行榜容器

    [Header("New Score Screen")]

    [SerializeField] private Canvas newScoringScreenCanvas;     //获得更高得分界面
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private InputField playerNameInputField;   //Player名字输入框

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ShowRandomBackground();     //显示随机背景

        if (ScoreManager.Instance.HasNewHighScore)  //得分进前10
        {
            ShowNewHighScoreScreen();     //显示取得更高分界面
        }
        else
        {
            ShowScoringScreen();    //显示计分界面
        }
        
        //添加到按钮功能字典
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(submitButton.gameObject.name, OnSubmitButtonClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(cancelButton.gameObject.name, HideNewHighScoreScreen);

        GameManager.GameState = GameState.Scoring;  //计分状态
    }

    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    /// <summary>
    /// 显示随机背景图片
    /// </summary>
    private void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];     //随机设置背景图片
    }

    /// <summary>
    /// 隐藏获得更高分数界面
    /// </summary>
    private void HideNewHighScoreScreen()
    {
        newScoringScreenCanvas.enabled = false;     //隐藏画布
        ScoreManager.Instance.SavePlayerScoreData();    //保存分数数据
        ShowRandomBackground();     //显示随机背景
        ShowScoringScreen();        //显示计分画面
    }

    /// <summary>
    /// 显示获得更高分数界面
    /// </summary>
    private void ShowNewHighScoreScreen()
    {
        newScoringScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(cancelButton);    //选中cancelButton
    }

    /// <summary>
    /// 显示计分画面 
    /// </summary>
    private void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();  //显示分数
        UIInput.Instance.SelectUI(mainMenuButton);      //选中主菜单按钮

        UpdateHighScoreLeaderBoard();   //更新分数排行榜
    }


    /// <summary>
    /// 更新分数排行榜
    /// </summary>
    private void UpdateHighScoreLeaderBoard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().scoreList;    //读取玩家得分数据，获得得分列表

        //遍历排行榜UI子级
        for (int i = 0; i < highScoreLeaderBoardContainer.childCount; i++)
        {
            var child = highScoreLeaderBoardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();                      //更新排名
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();    //更新分数
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName.ToString();//更新玩家名字
        }
    }

    /// <summary>
    /// 主菜单按钮
    /// </summary>
    private void OnMainMenuButtonClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();   //加载主菜单场景
    }

    /// <summary>
    /// 提交输入框名字
    /// </summary>
    private void OnSubmitButtonClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))   //输入框不为空
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);     //设置玩家名字
        }

        HideNewHighScoreScreen();   //隐藏获得更高分数界面
    }
}
