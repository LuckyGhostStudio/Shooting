using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private void OnEnable()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClick);
    }

    private void OnDisable()
    {
        startGameButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        Time.timeScale = 1;
        GameManager.GameState = GameState.Playing;
    }

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    private void OnStartGameButtonClick()
    {
        SceneLoader.Instance.LoadGameplayScene();   //����Gameplay����
    }
}
