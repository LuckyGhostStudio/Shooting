using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioSource sFXPlayer;     //��Ч������

    private const float MIN_PITCH = 0.9f;       //��С����
    private const float MAX_PITCH = 1.1f;       //�������

    /// <summary>
    /// ������Ч���ʺ�UI��Ч
    /// </summary>
    /// <param name="audioData">��Ƶ����</param>
    public void PlaySFX(AudioData audioData)
    {
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    /// <summary>
    /// �������������Ч���ʺ�������ظ���Ч
    /// </summary>
    /// <param name="audioData">��Ƶ����</param>
    public void PlayRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);   //�����������
        PlaySFX(audioData);
    }

    /// <summary>
    /// ���������Ч
    /// </summary>
    /// <param name="audioDatas">��Ƶ����</param>
    public void PlayRandomSFX(AudioData[] audioDatas)
    {
        PlayRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);  //���������Ч
    }
}

[System.Serializable]
public class AudioData
{
    public AudioClip audioClip;     //��Ƶ����
    [Range(0, 1)] public float volume;            //����
}
