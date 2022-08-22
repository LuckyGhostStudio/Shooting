using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    private static Text missileAmountText;      //导弹数量文本
    private static Image cooldownImage;         //冷却图片

    private void Awake()
    {
        missileAmountText = transform.Find("Amount Text").GetComponent<Text>();
        cooldownImage = transform.Find("CoolDown Image").GetComponent<Image>();
    }

    /// <summary>
    /// 更新导弹数量文本
    /// </summary>
    /// <param name="amount">分数</param>
    public static void UpdateAmountText(int amount) => missileAmountText.text = "× " + amount;

    /// <summary>
    /// 更新冷却图片填充值
    /// </summary>
    /// <param name="fillAmount">填充值</param>
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
}
