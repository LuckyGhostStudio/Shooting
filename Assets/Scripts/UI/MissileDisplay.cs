using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    private static Text missileAmountText;      //���������ı�
    private static Image cooldownImage;         //��ȴͼƬ

    private void Awake()
    {
        missileAmountText = transform.Find("Amount Text").GetComponent<Text>();
        cooldownImage = transform.Find("CoolDown Image").GetComponent<Image>();
    }

    /// <summary>
    /// ���µ��������ı�
    /// </summary>
    /// <param name="amount">����</param>
    public static void UpdateAmountText(int amount) => missileAmountText.text = "�� " + amount;

    /// <summary>
    /// ������ȴͼƬ���ֵ
    /// </summary>
    /// <param name="fillAmount">���ֵ</param>
    public static void UpdateCooldownImage(float fillAmount) => cooldownImage.fillAmount = fillAmount;
}
