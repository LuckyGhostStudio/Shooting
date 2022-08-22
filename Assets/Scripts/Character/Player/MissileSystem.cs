using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private int defaultAmount = 5; //����Ĭ������
    [SerializeField] private float cooldownTime = 2f;   //��ȴʱ��

    [SerializeField] private GameObject missile;    //����Ԥ����
    [SerializeField] private AudioData launchSFX;   //������Ч

    private bool isReady = true;    //�Ƿ���ȴ����

    private int amount; //��������

    private void Awake()
    {
        amount = defaultAmount;
    }

    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);    //���µ��������ı�UI
    }

    /// <summary>
    /// ���䵼��
    /// </summary>
    /// <param name="muzzleTransform">ǹ��λ��</param>
    public void Launch(Transform muzzleTransform)
    {
        if (amount == 0 || !isReady) return;

        isReady = false;

        PoolManager.Release(missile, muzzleTransform.position);     //�Ӷ�������õ���
        AudioManager.Instance.PlayRandomSFX(launchSFX);      //���ŷ�����Ч

        amount--;
        MissileDisplay.UpdateAmountText(amount);    //���µ�������UI

        if (amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1);      //������ȴͼƬ
        }
        else
        {
            StartCoroutine(nameof(CooldownCoroutine));  //������ȴ
        }
    }

    /// <summary>
    /// ������ȴ
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownCoroutine()
    {
        float cooldownValue = cooldownTime;

        while (cooldownValue > 0)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);   //������ȴͼƬ��ʾ
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0);       //��ǰ��ȴʱ���С

            yield return null;
        }

        isReady = true; //��ȴ����
    }
}
