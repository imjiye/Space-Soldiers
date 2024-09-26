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
        "-TIP-\r\nó�� ������ �����ѰŶ�� �� Ʃ�丮���� ���ּ���.",
        "-TIP-\r\nGuest�� �α��� �ߴٸ� \r\n���� �̸��Ϸ� ���� �����Ͻñ� ��õ�մϴ�.",
        "-TIP-\r\n����ִ� ��ɵ��� ���Ŀ� ������Ʈ �����Դϴ�.",
        "-TIP-\r\n������ �տ� �;��ٸ� Ÿ�Ӿ����� �����غ�����."
    };

    private void Start()
    {
        progressBar.minValue = 0;
        progressBar.maxValue = 100; // 0~100 ������ ����
        StartCoroutine(LoadScene());
        StartCoroutine(ChangeRandomTipText());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    // �ε� ȭ�鿡 ���� �ؽ�Ʈ�� �ֱ������� �����ϴ� �ڷ�ƾ
    IEnumerator ChangeRandomTipText()
    {
        while (true)
        {
            // �������� �ؽ�Ʈ ����
            int randomIndex = Random.Range(0, loadingTips.Length);
            randomTipText.text = loadingTips[randomIndex];

            // �ؽ�Ʈ ���� ���ݸ�ŭ ���
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
                // op.progress�� 0~90���� ��ȯ�Ͽ� Slider�� TextMeshProUGUI ������Ʈ
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
                // op.progress�� 0.9 �̻��̸� Slider�� 100���� ����
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
