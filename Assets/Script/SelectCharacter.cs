using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public Charcter charcter;
    public GameObject particle;
    public GameObject status;
    public SelectCharacter[] chars;
    public AudioClip AudioClip;
    public AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        particle.SetActive(false);
        status.SetActive(false);
        AudioSource.Stop();
        if (DataManager.instance.CurCharcter == charcter)
        {
            OnSelect();
        }
        else
        {
            OnDeSelect();
        }
    }

    private void OnMouseUpAsButton()
    {
        DataManager.instance.CurCharcter = charcter;
        
        OnSelect();

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] != this)
            {
                chars[i].OnDeSelect();
            }
        }
    }

    void OnDeSelect()
    {
        particle.SetActive(false);
        AudioSource.Stop();
        status.SetActive(false);
    }

    void OnSelect()
    {
        particle.SetActive(true);
        AudioSource.PlayOneShot(AudioClip);
        status.SetActive(true);
    }
}
