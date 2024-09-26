using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject[] cameras;
    public GameObject[] ShotButtons;
    public GameObject[] RespawnButtons;
    public GameObject[] ItemSpawners;

    //public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //player = Instantiate(charPrefabs[(int)DataManager.instance.CurCharcter]);
        for(int i = 0; i < charPrefabs.Length; i++)
        {
            if ((int)DataManager.instance.CurCharcter == i)
            {
                charPrefabs[i].SetActive(true);
                cameras[i].SetActive(true);
                ShotButtons[i].SetActive(true);
                RespawnButtons[i].SetActive(true);
                ItemSpawners[i].SetActive(true);
            }
            else
            {
                charPrefabs[i].SetActive(false);
                cameras[i].SetActive(false);
                ShotButtons[i].SetActive(false);
                RespawnButtons[i].SetActive(false);
                ItemSpawners[i].SetActive(false);
            }
        }
    }

}
