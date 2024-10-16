using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource BGMsource;
    //public AudioClip BGMclip;
    public static BGMManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<BGMManager>();
            }
            return m_instance;
        }
    }

    private static BGMManager m_instance;

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
        BGMsource.mute = true;
    }

    // BGM ���Ұ� ����
    public void BGMSoundAllOn()
    {
        BGMsource.mute = false;
    }
}
