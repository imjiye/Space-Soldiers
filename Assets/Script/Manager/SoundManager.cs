using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioClip[] SFXClips;

    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
            }
            return m_instance;
        }
    }

    private static SoundManager m_instance;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            InitSFXClips();
        }
    }

    private void InitSFXClips()
    {
        foreach (var clip in SFXClips)
        {
            sfxDict.Add(clip.name, clip); 
        }
    }

    // 효과음 음소거
    public void SFXSoundAllMute()
    {
        SFXSource.mute = true;
    }

    // 효과음 음소거 해제
    public void SFXSoundAllOn()
    {
        SFXSource.mute = false;
    }

    // SFX 재생
    public void PlaySFX(string sfxName)
    {
        if (sfxDict.ContainsKey(sfxName))
        {
            SFXSource.clip = sfxDict[sfxName];
            SFXSource.PlayOneShot(SFXSource.clip); 
        }
        else
        {
            Debug.LogWarning("SFX 클립을 찾을 수 없습니다: " + sfxName);
        }
    }

    // SFX 멈추기
    public void StopSFX()
    {
        SFXSource.Stop();
    }
}