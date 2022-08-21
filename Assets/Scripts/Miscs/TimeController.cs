using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0, 1)] private float bulletTimeScale = 0.1f;     //子弹时间缩放值

    private float defaultFixedDeltaTime;    //默认固定帧时间

    private float timeScaleBeforePause;     //暂停之前的时间缩放值

    private float t;

    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// 暂停：修改timeScale
    /// </summary>
    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;  //记录此刻的时间缩放值
        Time.timeScale = 0;
    }

    /// <summary>
    /// 取消暂停：恢复timeScale
    /// </summary>
    public void Unpause()
    {
        Time.timeScale = timeScaleBeforePause;  //恢复暂停前的时间缩放值
    }

    /// <summary>
    /// 进入子弹时间：改变时间流逝速度
    /// </summary>
    /// <param name="duration">持续时间</param>
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;   //修改时间流速
        StartCoroutine(SlowOutCoroutine(duration));     //退出子弹时间
    }

    /// <summary>
    /// 子弹时间：进入-退出
    /// </summary>
    /// <param name="inDuration">进入持续时间</param>
    /// <param name="outDuration">退出持续时间</param>
    public void BulletTime(float inDuration, float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));     //子弹时间
    }

    /// <summary>
    /// 子弹时间：进入-保持-退出
    /// </summary>
    /// <param name="inDuration">进入持续时间</param>
    /// <param name="keepingDuration">保持时间</param>
    /// <param name="outDuration">退出持续时间</param>
    public void BulletTime(float inDuration, float keepingDuration, float outDuration)
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(inDuration, keepingDuration, outDuration));     //子弹时间:进入-保持-退出
    }

    /// <summary>
    /// 时间变慢
    /// </summary>
    /// <param name="duration">持续时间</param>
    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));   //进入子弹时间
    }

    /// <summary>
    /// 时间恢复
    /// </summary>
    /// <param name="duration">持续时间</param>
    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));  //退出子弹时间
    }

    /// <summary>
    /// 进入子弹时间保持一段时间退出
    /// </summary>
    /// <param name="inDuration">进入持续时间</param>
    /// <param name="keepingDuration">保持时间</param>
    /// <param name="outDuration">退出持续时间</param>
    /// <returns></returns>
    IEnumerator SlowInKeepAndOutCoroutine(float inDuration, float keepingDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));   //进入子弹时间

        yield return new WaitForSecondsRealtime(keepingDuration);   //保持一段时间

        StartCoroutine(SlowOutCoroutine(outDuration));  //退出子弹时间
    }

    /// <summary>
    /// 进入退出子弹时间
    /// </summary>
    /// <param name="inDuration">进入持续时间</param>
    /// <param name="outDuration">退出持续时间</param>
    /// <returns></returns>
    IEnumerator SlowInAndOutCoroutine(float inDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));   //进入子弹时间

        StartCoroutine(SlowOutCoroutine(outDuration));  //退出子弹时间
    }

    /// <summary>
    /// 进入子弹时间：时间缩放值变慢
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <returns></returns>
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)  //非暂停状态
            {
                t += Time.unscaledDeltaTime / duration;      //unscaledDeltaTime不受timeScale的影响
                Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;   //改变固定帧间隔
            }

            yield return null;
        }
    }

    /// <summary>
    /// 退出子弹时间：时间缩放值恢复到正常
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <returns></returns>
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0;

        while (t < 1f)
        {
            if (GameManager.GameState != GameState.Paused)  //非暂停状态
            {
                t += Time.unscaledDeltaTime / duration;      //unscaledDeltaTime不受timeScale的影响
                Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);    //将时间缩放值 恢复到1
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;   //改变固定帧间隔
            }

            yield return null;
        }
    }
}
