using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private int scorePoint = 100;  //������

    [SerializeField] private int deathEnergyBonus = 3;  //��������ֵ

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))     //��������Player
        {
            player.Die();   //Player����
            Die();          //��������
        }
    }

    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);         //���ӵ÷�
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);     //��û�ɱ����ֵ
        EnemyManager.Instance.RemoveFromEnemyList(gameObject);      //����ǰEnemy���б��Ƴ�
        base.Die();
    }
}
