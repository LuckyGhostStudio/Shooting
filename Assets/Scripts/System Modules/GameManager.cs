using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public static GameState GameState { get { return Instance.gameState; } set { Instance.gameState = value; } }

    [SerializeField] private GameState gameState = GameState.Playing;   //��Ϸ״̬ 
}

public enum GameState
{
    Playing,    //����
    Paused,     //��ͣ
    GameOver    //��Ϸ����
}
