using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialControll : MonoBehaviour
{
    public GameObject StartNPC;
    public GameObject NPC;
    public GameObject[] TutorialText;

    private Coroutine tutorialCoroutine; // ���� ���� ���� �ڷ�ƾ�� ����

    public GameObject nextButton;
    public GameObject HomeButton;

    private bool isWaitingForNext = false; // ��ư ���� ���θ� Ȯ���ϱ� ���� �÷���

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartTutorial");
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(2f);
        StartNPC.SetActive(false);
        NPC.SetActive(true);
        nextButton.SetActive(true);

        // TutorialTexts�� Coroutine���� ����
        tutorialCoroutine = StartCoroutine(TutorialTexts());
    }

    // Coroutine���� �����Ͽ� �ϳ��� ���������� �ؽ�Ʈ�� ǥ�õǵ���
    IEnumerator TutorialTexts()
    {
        Debug.Log("TutorialTexts");

        for (int i = 0; i < TutorialText.Length; i++)
        {
            TutorialText[i].SetActive(true);

            // ��ư�� ������ ������ ���
            isWaitingForNext = true; // ��ư�� ���� ������ ���
            float waitTime = 0f;

            // 6�� �Ǵ� ��ư Ŭ�� ���
            while (isWaitingForNext && waitTime < 12f)
            {
                waitTime += Time.deltaTime;
                yield return null; // �� ������ ���
            }

            // �ؽ�Ʈ ��Ȱ��ȭ
            TutorialText[i].SetActive(false);
        }
        NPC.SetActive(false);
        nextButton.SetActive(false);
        HomeButton.SetActive(true);
    }

    // ��ư Ŭ���� ��� ���� �ؽ�Ʈ�� �Ѿ����
    public void OnNextButtonClicked()
    {
        if (isWaitingForNext)
        {
            isWaitingForNext = false; // ��� ���� ���¸� ����
        }
    }

    // �� ��ȯ
    public void LoadScene(string sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
