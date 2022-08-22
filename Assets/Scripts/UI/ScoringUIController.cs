using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [SerializeField] private Image background;  //����
    [SerializeField] private Sprite[] backgroundImages;

    [Header("Score Screen")]

    [SerializeField] private Canvas scoringScreenCanvas;    //�Ʒֽ��滭��
    [SerializeField] private Text playerScoreText;          //�����ı�
    [SerializeField] private Button mainMenuButton;         //���˵���ť

    [SerializeField] private Transform highScoreLeaderBoardContainer;   //�߷����а�����

    [Header("New Score Screen")]

    [SerializeField] private Canvas newScoringScreenCanvas;     //��ø��ߵ÷ֽ���
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private InputField playerNameInputField;   //Player���������

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ShowRandomBackground();     //��ʾ�������

        if (ScoreManager.Instance.HasNewHighScore)  //�÷ֽ�ǰ10
        {
            ShowNewHighScoreScreen();     //��ʾȡ�ø��߷ֽ���
        }
        else
        {
            ShowScoringScreen();    //��ʾ�Ʒֽ���
        }
        
        //��ӵ���ť�����ֵ�
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(submitButton.gameObject.name, OnSubmitButtonClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(cancelButton.gameObject.name, HideNewHighScoreScreen);

        GameManager.GameState = GameState.Scoring;  //�Ʒ�״̬
    }

    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    /// <summary>
    /// ��ʾ�������ͼƬ
    /// </summary>
    private void ShowRandomBackground()
    {
        background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];     //������ñ���ͼƬ
    }

    /// <summary>
    /// ���ػ�ø��߷�������
    /// </summary>
    private void HideNewHighScoreScreen()
    {
        newScoringScreenCanvas.enabled = false;     //���ػ���
        ScoreManager.Instance.SavePlayerScoreData();    //�����������
        ShowRandomBackground();     //��ʾ�������
        ShowScoringScreen();        //��ʾ�Ʒֻ���
    }

    /// <summary>
    /// ��ʾ��ø��߷�������
    /// </summary>
    private void ShowNewHighScoreScreen()
    {
        newScoringScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(cancelButton);    //ѡ��cancelButton
    }

    /// <summary>
    /// ��ʾ�Ʒֻ��� 
    /// </summary>
    private void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();  //��ʾ����
        UIInput.Instance.SelectUI(mainMenuButton);      //ѡ�����˵���ť

        UpdateHighScoreLeaderBoard();   //���·������а�
    }


    /// <summary>
    /// ���·������а�
    /// </summary>
    private void UpdateHighScoreLeaderBoard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().scoreList;    //��ȡ��ҵ÷����ݣ���õ÷��б�

        //�������а�UI�Ӽ�
        for (int i = 0; i < highScoreLeaderBoardContainer.childCount; i++)
        {
            var child = highScoreLeaderBoardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();                      //��������
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();    //���·���
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName.ToString();//�����������
        }
    }

    /// <summary>
    /// ���˵���ť
    /// </summary>
    private void OnMainMenuButtonClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();   //�������˵�����
    }

    /// <summary>
    /// �ύ���������
    /// </summary>
    private void OnSubmitButtonClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))   //�����Ϊ��
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);     //�����������
        }

        HideNewHighScoreScreen();   //���ػ�ø��߷�������
    }
}
