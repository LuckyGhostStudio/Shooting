using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private int defaultAmount = 5; //导弹默认数量
    [SerializeField] private float cooldownTime = 2f;   //冷却时间

    [SerializeField] private GameObject missile;    //导弹预制体
    [SerializeField] private AudioData launchSFX;   //发射音效

    private bool isReady = true;    //是否冷却结束

    private int amount; //导弹数量

    private void Awake()
    {
        amount = defaultAmount;
    }

    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);    //更新导弹数量文本UI
    }

    /// <summary>
    /// 发射导弹
    /// </summary>
    /// <param name="muzzleTransform">枪口位置</param>
    public void Launch(Transform muzzleTransform)
    {
        if (amount == 0 || !isReady) return;

        isReady = false;

        PoolManager.Release(missile, muzzleTransform.position);     //从对象池启用导弹
        AudioManager.Instance.PlayRandomSFX(launchSFX);      //播放发射音效

        amount--;
        MissileDisplay.UpdateAmountText(amount);    //更新导弹数量UI

        if (amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1);      //重置冷却图片
        }
        else
        {
            StartCoroutine(nameof(CooldownCoroutine));  //导弹冷却
        }
    }

    /// <summary>
    /// 导弹冷却
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownCoroutine()
    {
        float cooldownValue = cooldownTime;

        while (cooldownValue > 0)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);   //更新冷却图片显示
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0);       //当前冷却时间减小

            yield return null;
        }

        isReady = true; //冷却结束
    }
}
