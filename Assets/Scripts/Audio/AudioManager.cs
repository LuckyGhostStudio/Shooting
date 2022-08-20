using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioSource sFXPlayer;     //音效播放器

    private const float MIN_PITCH = 0.9f;       //最小音高
    private const float MAX_PITCH = 1.1f;       //最大音高

    /// <summary>
    /// 播放音效：适合UI音效
    /// </summary>
    /// <param name="audioData">音频数据</param>
    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    /// <summary>
    /// 播放随机音高音效：适合射击等重复音效
    /// </summary>
    /// <param name="audioData">音频数据</param>
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);   //设置随机音高
        PlaySFX(audioData);
    }

    /// <summary>
    /// 随机播放音效
    /// </summary>
    /// <param name="audioDatas">音频数组</param>
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);  //随机播放音效
    }
}

[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;     //音频剪辑
    [Range(0, 1)] public float volume;            //音量
}
