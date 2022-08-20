using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private int scorePoint = 100;  //������

    [SerializeField] private int deathEnergyBonus = 3;  //��������ֵ

    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);         //���ӵ÷�
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);     //��û�ɱ����ֵ
        EnemyManager.Instance.RemoveFromEnemyList(gameObject);      //����ǰEnemy���б��Ƴ�
        base.Die();
    }
}
