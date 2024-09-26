using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] BGMsources;
    public AudioSource[] SFXSources;

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
    }

    // BMG 음소거
    public void BGMSoundAllMute()
    {
        for (int i = 0; i < BGMsources.Length; i++)
        {
            BGMsources[i].mute = true;
        }
    }

    // BGM 음소거 해제
    public void BGMSoundAllOn()
    {
        for (int i = 0; i < BGMsources.Length; i++)
        {
            BGMsources[i].mute = false;
        }
    }

    // 효과음 음소거
    public void SFXSoundAllMute()
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            SFXSources[i].mute = true;
        }
    }

    // 효과음 음소거 해제
    public void SFXSoundAllOn()
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            SFXSources[i].mute = false;
        }
    }

    // 효과음 멈추기
    public void stopSound(string soundName)
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            if (SFXSources[i].gameObject.name.CompareTo(soundName) == 0)
            {
                SFXSources[i].Stop();
            }
        }
    }

    // 효과음 플레이
    public void PlaySound(string soundName)
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            if (SFXSources[i].gameObject.name.CompareTo(soundName) == 0)
            {
                SFXSources[i].Play();
            }
        }
    }
}