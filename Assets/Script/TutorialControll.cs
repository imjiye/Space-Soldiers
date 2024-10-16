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

    private Coroutine tutorialCoroutine; // 현재 진행 중인 코루틴을 저장

    public GameObject nextButton;
    public GameObject HomeButton;

    private bool isWaitingForNext = false; // 버튼 눌림 여부를 확인하기 위한 플래그

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

        // TutorialTexts를 Coroutine으로 실행
        tutorialCoroutine = StartCoroutine(TutorialTexts());
    }

    // Coroutine으로 변경하여 하나씩 순차적으로 텍스트가 표시되도록
    IEnumerator TutorialTexts()
    {
        Debug.Log("TutorialTexts");

        for (int i = 0; i < TutorialText.Length; i++)
        {
            TutorialText[i].SetActive(true);
            audioSources.clip = audioClips[i];
            audioSources.Play();

            // 오디오 클립의 길이 가져오기
            float audioLength = audioSources.clip.length;

            // 버튼을 누르기 전까지 대기
            isWaitingForNext = true;
            float waitTime = 0f;

            // 오디오 클립의 길이와 18초 중에서 더 짧은 시간 동안 대기
            while (isWaitingForNext && waitTime < Mathf.Min(audioLength, 18f))
            {
                waitTime += Time.deltaTime;
                yield return null; // 매 프레임 대기
            }

            // 텍스트 비활성화
            TutorialText[i].SetActive(false);
        }

        NPC.SetActive(false);
        nextButton.SetActive(false);
        audioSources.Stop();
        SoundManager.instance.PlaySFX("TutorialClear");
        HomeButton.SetActive(true);
    }

    // 버튼 클릭시 즉시 다음 텍스트로 넘어가도록
    public void OnNextButtonClicked()
    {
        if (isWaitingForNext)
        {
            isWaitingForNext = false; // 대기 중인 상태를 종료
        }
    }

    // 씬 전환
    public void LoadScene(string sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}