using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    Slider progressBar;

    [SerializeField]
    TextMeshProUGUI progressText;

    [SerializeField]
    TextMeshProUGUI randomTipText;

    [SerializeField]
    float tipChangeInterval = 3f;

    string[] loadingTips = new string[]
    {
        "-TIP-\r\n처음 게임을 시작한거라면 꼭 튜토리얼을 봐주세요.",
        "-TIP-\r\nGuest로 로그인 했다면 \r\n빨리 이메일로 계정 연동하시길 추천합니다.",
        "-TIP-\r\n잠겨있는 기능들은 추후에 업데이트 예정입니다.",
        "-TIP-\r\n게임이 손에 익었다면 타임어택을 도전해보세요."
    };

    private void Start()
    {
        progressBar.minValue = 0;
        progressBar.maxValue = 100; // 0~100 범위로 설정
        StartCoroutine(LoadScene());
        StartCoroutine(ChangeRandomTipText());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    // 로딩 화면에 랜덤 텍스트를 주기적으로 변경하는 코루틴
    IEnumerator ChangeRandomTipText()
    {
        while (true)
        {
            // 랜덤으로 텍스트 선택
            int randomIndex = Random.Range(0, loadingTips.Length);
            randomTipText.text = loadingTips[randomIndex];

            // 텍스트 변경 간격만큼 대기
            yield return new WaitForSeconds(tipChangeInterval);
        }
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                // op.progress를 0~90으로 변환하여 Slider와 TextMeshProUGUI 업데이트
                float progressValue = Mathf.Lerp(progressBar.value, op.progress * 100, timer);
                progressBar.value = progressValue;
                progressText.text = Mathf.FloorToInt(progressValue) + "%";

                if (progressBar.value >= op.progress * 100)
                {
                    timer = 0f;
                }
            }
            else
            {
                // op.progress가 0.9 이상이면 Slider를 100으로 설정
                progressBar.value = Mathf.Lerp(progressBar.value, 100f, timer);
                progressText.text = Mathf.FloorToInt(progressBar.value) + "%";

                if (progressBar.value == 100f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
