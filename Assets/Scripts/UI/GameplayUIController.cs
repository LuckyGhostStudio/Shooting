using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [Header("Audio Data")]

    [SerializeField] private AudioData pauseSFX;    //��ͣ��Ч
    [SerializeField] private AudioData unpauseSFX;  //ȡ����ͣ��Ч

    [Header("Canvas")]

    [SerializeField] private Canvas hUDCanvas;      //HUB����UI
    [SerializeField] private Canvas menusCanvas;    //��ͣ�˵�UI

    [Header("Button")]

    [SerializeField] private Button resumeButton;   //ȡ����ͣ��ť
    [SerializeField] private Button optionsButton;  //ѡ�ť
    [SerializeField] private Button mainMenuButton; //���˵���ť

    int buttonPressedParameterID = Animator.StringToHash("Pressed");    //Pressed��ID

    private void OnEnable()
    {
        //�����¼�
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        //����ť���ֺ��书�ܺ�����ӵ��ֵ�
        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        //ȡ�������¼�
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        ButtonPressedBehavior.buttonFunctionTable.Clear();  //��հ�ť���ܺ����ֵ�
    }

    /// <summary>
    /// ��ͣ
    /// </summary>
    private void Pause()
    {
        TimeController.Instance.Pause();            //��ͣ
        //hUDCanvas.enabled = false;                //����HUD UI
        menusCanvas.enabled = true;                 //������ͣ�˵�UI
        GameManager.GameState = GameState.Paused;   //��ͣ״̬
        playerInput.EnablePauseMenuInput();         //�л���PauseMenu������
        playerInput.SwitchToDynamicUpdateMode();    //�л�����̬����ģʽ
        UIInput.Instance.SelectUI(resumeButton);    //ѡ�з��ذ�ť
        AudioManager.Instance.PlaySFX(pauseSFX);    //������ͣ��Ч
    }

    /// <summary>
    /// ȡ����ͣ
    /// </summary>
    private void Unpause()
    {
        resumeButton.Select();  //ѡ�а�ť
        resumeButton.animator.SetTrigger(buttonPressedParameterID);    //״̬�л�Ϊ����
        UIInput.Instance.DisableAllUIInput();
        AudioManager.Instance.PlaySFX(unpauseSFX);    //����ȡ����ͣ��Ч
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    private void OnResumeButtonClick()
    {
        TimeController.Instance.Unpause();      //ȡ����ͣ
        //hUDCanvas.enabled = true;               //����HUD UI
        menusCanvas.enabled = false;            //������ͣ�˵�UI
        GameManager.GameState = GameState.Playing;  //����
        playerInput.EnableGameplayInput();      //�л���Gameplay������
        playerInput.SwitchToFixedUpdateMode();  //�л����̶�����ģʽ
        UIInput.Instance.SelectUI(resumeButton);    //ѡ�з��ذ�ť
        UIInput.Instance.DisableAllUIInput();
    }

    /// <summary>
    /// ѡ��
    /// </summary>
    private void OnOptionsButtonClick()
    {
        //��ʾѡ��˵�
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    /// <summary>
    /// �ص����˵�
    /// </summary>
    private void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;    //������ͣ�˵�
        SceneLoader.Instance.LoadMainMenuScene();   //�������˵�
    }
}
