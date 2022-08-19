using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBarHUD : StateBar
{
    [SerializeField] private Text valueText;    //״̬���ı�
    private int maxValue; 

    /// <summary>
    /// ����״̬���ı�
    /// </summary>
    void SetTextValue()
    {
        valueText.text = (targetFillAmount * 100).ToString() + " / " + maxValue.ToString();
    }

    public override void Initialize(int currentValue, int maxValue)
    {
        base.Initialize(currentValue, maxValue);
        this.maxValue = maxValue;
        SetTextValue();
    }

    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetTextValue();
        return base.BufferedFillingCoroutine(image);
    }
}
