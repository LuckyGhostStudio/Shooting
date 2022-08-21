using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public static GameState GameState { get { return Instance.gameState; } set { Instance.gameState = value; } }

    [SerializeField] private GameState gameState = GameState.Playing;   //游戏状态 
}

public enum GameState
{
    Playing,    //运行
    Paused,     //暂停
    GameOver    //游戏结束
}
