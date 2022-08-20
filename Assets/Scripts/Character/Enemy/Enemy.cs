using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private int scorePoint = 100;  //分数点

    [SerializeField] private int deathEnergyBonus = 3;  //死亡能量值

    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);         //增加得分
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);     //获得击杀能量值
        EnemyManager.Instance.RemoveFromEnemyList(gameObject);      //将当前Enemy从列表移除
        base.Die();
    }
}
