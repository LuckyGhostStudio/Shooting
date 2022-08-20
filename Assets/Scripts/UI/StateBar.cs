using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    [SerializeField] private Image fillImageBack;       //状态条填充缓冲条
    [SerializeField] private Image fillImageFront;      //状态条填充前景：实际值
    [SerializeField] private bool delayFill = true;     //是否需要延迟填充
    [SerializeField] private float fileDelayTime = 0.5f;     //填充延迟时间
    [SerializeField] private float fillSpeed = 0.1f;    //填充缓冲速度

    private float currentFileAmount;    //当前填充值
    protected private float targetFillAmount;     //目标填充值
    private float t;    //插值时间

    private Canvas canvas;

    private WaitForSeconds waitForDelayFill;    //等待延迟填充

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
    /// 初始化状态条参数
    /// </summary>
    /// <param name="currentValue">当前值</param>
    /// <param name="maxValue">最大值</param>
    public virtual void Initialize(int currentValue, int maxValue)
    {
        currentFileAmount = (float)currentValue / maxValue;
        targetFillAmount = currentFileAmount;
        fillImageBack.fillAmount = currentFileAmount;
        fillImageFront.fillAmount = currentFileAmount;
    }

    /// <summary>
    /// 更新状态条
    /// </summary>
    /// <param name="currentValue">当前值</param>
    /// <param name="maxValue">最大值</param>
    public void UpdateState(int currentValue, int maxValue)
    {
        targetFillAmount = (float)currentValue / maxValue;

        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }

        if (currentFileAmount > targetFillAmount)   //状态条值减少时
        {
            fillImageFront.fillAmount = targetFillAmount;               //前景状态条设为目标值
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));    //缓冲条慢慢减少到目标值
        }
        else if (currentFileAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;                //缓冲状态条设为目标值
            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageFront));   //前景状态条慢慢增加到目标值
        }
    }

    /// <summary>
    /// 缓冲填充
    /// </summary>
    /// <param name="image">填充对象</param>
    /// <returns></returns>
    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)  //延迟填充
        {
            yield return waitForDelayFill;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFileAmount = Mathf.Lerp(currentFileAmount, targetFillAmount, t);     //当前填充值变化到目标填充值
            image.fillAmount = currentFileAmount;
            yield return null;
        }
    }
}
