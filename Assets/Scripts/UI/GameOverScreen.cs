using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private AudioData onConfirmGameOverSFX;    //ȷ��GameOver��Ч

    private Canvas canvas;      //GameOverScreen����
    private Animator animator;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");  //GameOver�˳�����ID

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;       //����GameOver�¼�
        playerInput.onConfirmGameOver += OnConfirmGameOver;     //ȷ��GameOver�¼�
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        playerInput.onConfirmGameOver -= OnConfirmGameOver;
    }

    /// <summary>
    /// ����GameOverʱ
    /// </summary>
    private void OnGameOver()
    {
        HUDCanvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        playerInput.DisableAllInput();      //������������
    }

    /// <summary>
    /// ȷ��GameOverʱ
    /// </summary>
    private void OnConfirmGameOver()
    {
        AudioManager.Instance.PlayRandomSFX(onConfirmGameOverSFX);  //����ȷ����Ϸ������Ч
        playerInput.DisableAllInput();              //������������
        animator.Play(exitStateID);                 //����GameOver�˳�����
        SceneLoader.Instance.LoadScoringScene();    //���ؼƷֳ���
    }

    /// <summary>
    /// �л���GameOverScreen������GameOverEnter�������Ž���ʱ���ã������¼���
    /// </summary>
    private void EnableGameOverScreenInput()
    {
        playerInput.EnableGameOverScreenInput();    //�л���GameOverScreen������
    }
}
