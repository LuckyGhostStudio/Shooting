using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)]; //随机返回一个场景中的敌人
    public int WaveNumber => waveNumber;                //波数：等价于 get{return waveNumber;}
    public float TimeBetweenWaves => timeBetweenWaves;  //每波间隔时间

    [SerializeField] private bool spawnEnemy = true;    //是否生成敌人
    [SerializeField] private GameObject waveUI;         //显示波数UI

    [SerializeField] private GameObject[] enemyPrefabs;     //enemy数组
    [SerializeField] private float timeBetweenSpawns = 1f;  //敌人生成间隔时间

    [SerializeField] private float timeBetweenWaves = 1f;   //每波敌人生成前等待时间

    [SerializeField] private int minEnemyAmount = 4;     //最小敌人数量
    [SerializeField] private int maxEnemyAmount = 10;    //最大敌人数量

    private int waveNumber = 1;  //敌人波数

    private int enemyAmount;    //敌人数量

    private List<GameObject> enemyList;     //存放当前场景中所有敌人

    WaitForSeconds waitTimeBetweenSpawns;
    WaitForSeconds waitTimeBetweenWaves;
    WaitUntil waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);      //等待直到敌人数量为0时：匿名函数
    }

    IEnumerator Start()
    {
        while (spawnEnemy)  //生成敌人
        {
            waveUI.SetActive(true);     //显示波数

            yield return waitTimeBetweenWaves;                              //等待一会

            waveUI.SetActive(false);    //结束显示

            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));    //生成一波敌人
        }
    }

    /// <summary>
    /// 随机生成一波敌人
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlySpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);    //最小敌人数量随波数增加

        //随机生成一波敌人
        for (int i = 0; i < enemyAmount; i++)
        {
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));    //随机从enemy数组中获得一个enemy，从对象池中启用，并添加到列表

            yield return waitTimeBetweenSpawns;
        }

        yield return waitUntilNoEnemy;                                  //等待直到敌人数量为0

        waveNumber++;   //波数++
    }

    /// <summary>
    /// 将enemy从列表移除
    /// </summary>
    /// <param name="enemy">已死亡的enemy</param>
    public void RemoveFromEnemyList(GameObject enemy) => enemyList.Remove(enemy);   //Lambda表达式
}
