using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private EnergyBar energyBar;

    private float overdriveInterval = 0.1f;     //能量爆发消耗能量时间间隔

    private bool available = true;      //能否获取能量

    public const int MAX = 100;        //最大能量值
    public const int PERCENT = 1;      //能量增加百分比

    private int energy;     //当前能量值

    WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }

    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    private void OnDisable()
    {
        PlayerOverdrive.on -= PlayerOverdriveOn;
        PlayerOverdrive.off -= PlayerOverdriveOff;
    }

    private void Start()
    {
        energyBar.Initialize(energy, MAX);  //初始化能量条
    }

    /// <summary>
    /// 获得能量
    /// </summary>
    /// <param name="value">值</param>
    public void Obtain(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;  //能量满 || 能量爆发时 || 玩家被消灭

        energy = Mathf.Clamp(energy + value, 0, MAX);   //增加能量值：限制范围 [0, MAX]
        energyBar.UpdateState(energy, MAX);     //更新能量条
    }

    /// <summary>
    /// 使用能量
    /// </summary>
    /// <param name="value">值</param>
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateState(energy, MAX);     //更新能量条

        if (energy == 0 && !available)  //能量耗尽 处于能量爆发时
        {
            PlayerOverdrive.off.Invoke();   //关闭能量爆发
        }
    }

    /// <summary>
    /// 当前能量值是否足够消耗
    /// </summary>
    /// <param name="value">消耗值</param>
    /// <returns></returns>
    public bool IsEnough(int value)
    {
        return energy >= value;
    }

    /// <summary>
    /// 开启过速
    /// </summary>
    private void PlayerOverdriveOn()
    {
        available = false;  //禁用获取能量
        StartCoroutine(nameof(KeepUsingCoroutine));     //持续消耗能量
    }

    /// <summary>
    /// 关闭过速
    /// </summary>
    private void PlayerOverdriveOff()
    {
        available = true;   //可以获取能量
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    /// <summary>
    /// 保持能量消耗
    /// </summary>
    /// <returns></returns>
    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitForOverdriveInterval;

            Use(PERCENT);   //消耗1能量
        }
    }
}
