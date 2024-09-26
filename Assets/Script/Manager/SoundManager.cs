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

    // BMG ���Ұ�
    public void BGMSoundAllMute()
    {
        for (int i = 0; i < BGMsources.Length; i++)
        {
            BGMsources[i].mute = true;
        }
    }

    // BGM ���Ұ� ����
    public void BGMSoundAllOn()
    {
        for (int i = 0; i < BGMsources.Length; i++)
        {
            BGMsources[i].mute = false;
        }
    }

    // ȿ���� ���Ұ�
    public void SFXSoundAllMute()
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            SFXSources[i].mute = true;
        }
    }

    // ȿ���� ���Ұ� ����
    public void SFXSoundAllOn()
    {
        for (int i = 0; i < SFXSources.Length; i++)
        {
            SFXSources[i].mute = false;
        }
    }

    // ȿ���� ���߱�
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

    // ȿ���� �÷���
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