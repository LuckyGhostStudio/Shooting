using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0, 1)] private float bulletTimeScale = 0.1f;     //�ӵ�ʱ������ֵ

    private float defaultFixedDeltaTime;    //Ĭ�Ϲ̶�֡ʱ��

    private float timeScaleBeforePause;     //��֮ͣǰ��ʱ������ֵ

    private float t;

    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// ��ͣ���޸�timeScale
    /// </summary>
    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;  //��¼�˿̵�ʱ������ֵ
        Time.timeScale = 0;
    }

    /// <summary>
    /// ȡ����ͣ���ָ�timeScale
    /// </summary>
    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;  //�ָ���ͣǰ��ʱ������ֵ
    }

    /// <summary>
    /// �����ӵ�ʱ�䣺�ı�ʱ�������ٶ�
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;   //�޸�ʱ������
        StartCoroutine(SlowOutCoroutine(duration));     //�˳��ӵ�ʱ��
    }

    /// <summary>
    /// �ӵ�ʱ�䣺����-�˳�
    /// </summary>
    /// <param name="inDuration">�������ʱ��</param>
    /// <param name="outDuration">�˳�����ʱ��</param>
    public void BulletTime(float inDuration, float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));     //�ӵ�ʱ��
    }

    /// <summary>
    /// �ӵ�ʱ�䣺����-����-�˳�
    /// </summary>
    /// <param name="inDuration">�������ʱ��</param>
    /// <param name="keepingDuration">����ʱ��</param>
    /// <param name="outDuration">�˳�����ʱ��</param>
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration, outDuration));     //�ӵ�ʱ��:����-����-�˳�
    }

    /// <summary>
    /// ʱ�����
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));   //�����ӵ�ʱ��
    }

    /// <summary>
    /// ʱ��ָ�
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));  //�˳��ӵ�ʱ��
    }

    /// <summary>
    /// �����ӵ�ʱ�䱣��һ��ʱ���˳�
    /// </summary>
    /// <param name="inDuration">�������ʱ��</param>
    /// <param name="keepingDuration">����ʱ��</param>
    /// <param name="outDuration">�˳�����ʱ��</param>
    /// <returns></returns>
    IEnumerator SlowInKeepAndOutCoroutine(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));   //�����ӵ�ʱ��

        yield return new WaitForSecondsRealtime(keepingDuration);   //����һ��ʱ��

        StartCoroutine(SlowOutCoroutine(outDuration));  //�˳��ӵ�ʱ��
    }

    /// <summary>
    /// �����˳��ӵ�ʱ��
    /// </summary>
    /// <param name="inDuration">�������ʱ��</param>
    /// <param name="outDuration">�˳�����ʱ��</param>
    /// <returns></returns>
    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));   //�����ӵ�ʱ��

        StartCoroutine(SlowOutCoroutine(outDuration));  //�˳��ӵ�ʱ��
    }

    /// <summary>
    /// �����ӵ�ʱ�䣺ʱ������ֵ����
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    /// <returns></returns>
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)  //����ͣ״̬
            {
                t += Time.unscaledDeltaTime / duration;      //unscaledDeltaTime����timeScale��Ӱ��
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;   //�ı�̶�֡���
            }

            yield return null;
        }
    }

    /// <summary>
    /// �˳��ӵ�ʱ�䣺ʱ������ֵ�ָ�������
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    /// <returns></returns>
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)  //����ͣ״̬
            {
                t += Time.unscaledDeltaTime / duration;      //unscaledDeltaTime����timeScale��Ӱ��
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);    //��ʱ������ֵ �ָ���1
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;   //�ı�̶�֡���
            }

            yield return null;
        }
    }
}
