using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)]; //�������һ�������еĵ���
    public int WaveNumber => waveNumber;                //�������ȼ��� get{return waveNumber;}
    public float TimeBetweenWaves => timeBetweenWaves;  //ÿ�����ʱ��

    [SerializeField] private bool spawnEnemy = true;    //�Ƿ����ɵ���
    [SerializeField] private GameObject waveUI;         //��ʾ����UI

    [SerializeField] private GameObject[] enemyPrefabs;     //enemy����
    [SerializeField] private float timeBetweenSpawns = 1f;  //�������ɼ��ʱ��

    [SerializeField] private float timeBetweenWaves = 1f;   //ÿ����������ǰ�ȴ�ʱ��

    [SerializeField] private int minEnemyAmount = 4;     //��С��������
    [SerializeField] private int maxEnemyAmount = 10;    //����������

    private int waveNumber = 1;  //���˲���

    private int enemyAmount;    //��������

    private List<GameObject> enemyList;     //��ŵ�ǰ���������е���

    WaitForSeconds waitTimeBetweenSpawns;
    WaitForSeconds waitTimeBetweenWaves;
    WaitUntil waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);      //�ȴ�ֱ����������Ϊ0ʱ����������
    }

    IEnumerator Start()
    {
        while (spawnEnemy)  //���ɵ���
        {
            waveUI.SetActive(true);     //��ʾ����

            yield return waitTimeBetweenWaves;                              //�ȴ�һ��

            waveUI.SetActive(false);    //������ʾ

            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));    //����һ������
        }
    }

    /// <summary>
    /// �������һ������
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomlySpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);    //��С���������沨������

        //�������һ������
        for (int i = 0; i < enemyAmount; i++)
        {
            enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));    //�����enemy�����л��һ��enemy���Ӷ���������ã�����ӵ��б�

            yield return waitTimeBetweenSpawns;
        }

        yield return waitUntilNoEnemy;                                  //�ȴ�ֱ����������Ϊ0

        waveNumber++;   //����++
    }

    /// <summary>
    /// ��enemy���б��Ƴ�
    /// </summary>
    /// <param name="enemy">��������enemy</param>
    public void RemoveFromEnemyList(GameObject enemy) => enemyList.Remove(enemy);   //Lambda���ʽ
}
