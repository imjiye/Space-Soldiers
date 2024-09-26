using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoading : MonoBehaviour
{
    private bool isButtonInput = false; // 버튼 눌림 여부를 확인하기 위한 플래그

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isButtonInput)
        {
            LoadingSceneManager.LoadScene("Stage1");
        }
    }

    public void LoadSceneInput()
    {
        isButtonInput = true;
    }
}
