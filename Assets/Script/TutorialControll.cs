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
    public AudioClip[] audioClips;
    public AudioSource audioSources;

    private Coroutine tutorialCoroutine; // ���� ���� ���� �ڷ�ƾ�� ����

    public GameObject nextButton;
    public GameObject HomeButton;

    private bool isWaitingForNext = false; // ��ư ���� ���θ� Ȯ���ϱ� ���� �÷���

    // Start is called before the first frame update

    private void OnEnable()
    {
        SoundManager.instance.PlaySFX("StartNPC_SFX");
        StartCoroutine("StartTutorial");
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(2f);
        StartNPC.SetActive(false);
        SoundManager.instance.StopSFX();
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
            audioSources.clip = audioClips[i];
            audioSources.Play();

            // ����� Ŭ���� ���� ��������
            float audioLength = audioSources.clip.length;

            // ��ư�� ������ ������ ���
            isWaitingForNext = true;
            float waitTime = 0f;

            // ����� Ŭ���� ���̿� 18�� �߿��� �� ª�� �ð� ���� ���
            while (isWaitingForNext && waitTime < Mathf.Min(audioLength, 18f))
            {
                waitTime += Time.deltaTime;
                yield return null; // �� ������ ���
            }

            // �ؽ�Ʈ ��Ȱ��ȭ
            TutorialText[i].SetActive(false);
        }

        NPC.SetActive(false);
        nextButton.SetActive(false);
        audioSources.Stop();
        SoundManager.instance.PlaySFX("TutorialClear");
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