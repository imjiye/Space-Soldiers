using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoading : MonoBehaviour
{
    private bool isButtonInput = false; // ��ư ���� ���θ� Ȯ���ϱ� ���� �÷���

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
