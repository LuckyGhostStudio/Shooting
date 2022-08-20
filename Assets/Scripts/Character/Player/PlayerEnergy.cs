using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private EnergyBar energyBar;

    private float overdriveInterval = 0.1f;     //����������������ʱ����

    private bool available = true;      //�ܷ��ȡ����

    public const int MAX = 100;        //�������ֵ
    public const int PERCENT = 1;      //�������Ӱٷֱ�

    private int energy;     //��ǰ����ֵ

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
        energyBar.Initialize(energy, MAX);  //��ʼ��������
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="value">ֵ</param>
    public void Obtain(int value)
    {
        if (energy == MAX || !available || !gameObject.activeSelf) return;  //������ || ��������ʱ || ��ұ�����

        energy = Mathf.Clamp(energy + value, 0, MAX);   //��������ֵ�����Ʒ�Χ [0, MAX]
        energyBar.UpdateState(energy, MAX);     //����������
    }

    /// <summary>
    /// ʹ������
    /// </summary>
    /// <param name="value">ֵ</param>
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateState(energy, MAX);     //����������

        if (energy == 0 && !available)  //�����ľ� ������������ʱ
        {
            PlayerOverdrive.off.Invoke();   //�ر���������
        }
    }

    /// <summary>
    /// ��ǰ����ֵ�Ƿ��㹻����
    /// </summary>
    /// <param name="value">����ֵ</param>
    /// <returns></returns>
    public bool IsEnough(int value)
    {
        return energy >= value;
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void PlayerOverdriveOn()
    {
        available = false;  //���û�ȡ����
        StartCoroutine(nameof(KeepUsingCoroutine));     //������������
    }

    /// <summary>
    /// �رչ���
    /// </summary>
    private void PlayerOverdriveOff()
    {
        available = true;   //���Ի�ȡ����
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <returns></returns>
    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && energy > 0)
        {
            yield return waitForOverdriveInterval;

            Use(PERCENT);   //����1����
        }
    }
}
