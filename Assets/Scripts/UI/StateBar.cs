using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    [SerializeField] private Image fillImageBack;       //״̬����仺����
    [SerializeField] private Image fillImageFront;      //״̬�����ǰ����ʵ��ֵ
    [SerializeField] private bool delayFill = true;     //�Ƿ���Ҫ�ӳ����
    [SerializeField] private float fileDelayTime = 0.5f;     //����ӳ�ʱ��
    [SerializeField] private float fillSpeed = 0.1f;    //��仺���ٶ�

    private float currentFileAmount;    //��ǰ���ֵ
    protected private float targetFillAmount;     //Ŀ�����ֵ
    private float t;    //��ֵʱ��

    private Canvas canvas;

    private WaitForSeconds waitForDelayFill;    //�ȴ��ӳ����

    private Coroutine bufferedFillingCoroutine;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        waitForDelayFill = new WaitForSeconds(fileDelayTime);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// ��ʼ��״̬������
    /// </summary>
    /// <param name="currentValue">��ǰֵ</param>
    /// <param name="maxValue">���ֵ</param>
    public virtual void Initialize(int currentValue, int maxValue)
    {
        currentFileAmount = (float)currentValue / maxValue;
        targetFillAmount = currentFileAmount;
        fillImageBack.fillAmount = currentFileAmount;
        fillImageFront.fillAmount = currentFileAmount;
    }

    /// <summary>
    /// ����״̬��
    /// </summary>
    /// <param name="currentValue">��ǰֵ</param>
    /// <param name="maxValue">���ֵ</param>
    public void UpdateState(int currentValue, int maxValue)
    {
        targetFillAmount = (float)currentValue / maxValue;

        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }

        if (currentFileAmount > targetFillAmount)   //״̬��ֵ����ʱ
        {
            fillImageFront.fillAmount = targetFillAmount;               //ǰ��״̬����ΪĿ��ֵ
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));    //�������������ٵ�Ŀ��ֵ
        }
        else if (currentFileAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;                //����״̬����ΪĿ��ֵ
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));   //ǰ��״̬���������ӵ�Ŀ��ֵ
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="image">������</param>
    /// <returns></returns>
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)  //�ӳ����
        {
            yield return waitForDelayFill;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFileAmount = Mathf.Lerp(currentFileAmount, targetFillAmount, t);     //��ǰ���ֵ�仯��Ŀ�����ֵ
            image.fillAmount = currentFileAmount;
            yield return null;
        }
    }
}
