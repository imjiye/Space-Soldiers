using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using UnityEngine.Audio;

// �ʿ��� UI�� ��� �����ϰ� ������ �� �ֵ��� ����ϴ� UI �Ŵ���
public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    private static UIManager m_instance;

    public GameObject Menu; // �޴� ��ư Ŭ���� Ȱ��ȭ�� �޴�â
    public GameObject Option; // �ɼ� ��ư Ŭ���� Ȱ��ȭ�� �ɼ�â
    public GameObject QuitPopUp; // ���� �����ư Ŭ���� Ȱ��ȭ�� ���â

    public Text ammoText; // ź�� ǥ��
    public Text scoreText; // ���� ǥ��
    public Text coinText; // ���� ǥ��
    public Text waveText; // �� ���̺� ǥ��
    public Text enemyText; // �����ִ� �� ǥ��
    public Text timeText; // �ð� ǥ��

    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ �� UI
    public GameObject gameclearUI; // ���� Ŭ����� Ȱ��ȭ �� UI 
    public GameObject ErrorPopUp; // ���� �˾�â UI
    public GameObject DateResetPopUp; // ������ ���� ��� â UI
    public GameObject ChatPopup; // ä��â UI

    public GameObject RankingPopUp; // ��ŷ �˾� UI
    public Text rank1NameText;
    public Text rank1ScoreText;
    public Text rank2NameText;
    public Text rank2ScoreText;
    public Text rank3NameText;
    public Text rank3ScoreText;

    // ����Ʈ ���ھ� ǥ��
    public Text O_bestScoreText; 
    public Text C_bestScoreText;
    // ȹ���� ���� ǥ��
    public Text O_CurScoreText; 
    public Text C_CurScoreText;
    // ȹ���� ���� ǥ��
    public Text O_CurCoinText;
    public Text C_CurCoinText;
    // ������ ���� ǥ��
    public Text O_myCoinText;
    public Text C_myCoinText;
    // DOTeween�� �̿��� ����Ʈ���ھ� ���� ��ȯ
    public GameObject O_doteweenColor; 
    public GameObject C_doteweenColor;

    public Text HaveCoin; // ���� �������� ���� ���� ǥ��

    public AudioMixer masterMixer; // ����� ������ ���� �ͼ�
    public Slider BGMSlider; // BGM ���� �����̴�
    public Slider SFXSlider; // ȿ���� ���� �����̴�

    // �ð� ������ �� �����ֱ�
    public void TimeUpdate(float surviveTime)
    {
        timeText.text = "Time : " + (int) surviveTime;
    }

    // ź�� �ؽ�Ʈ ����
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // ���� �ؽ�Ʈ ����
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    // ���� �ؽ�Ʈ ����
    public void UpdateCoinText(int newCoin)
    {
        coinText.text = " " + newCoin;
    }

    // �� ���̺� �ؽ�Ʈ ����
    public void UpdateWaveText(int waves)
    {
        waveText.text = "Wave : " + waves;
    }

    // ���� �� ���� ����
    public void UpdateEnemyText(int count)
    {
        enemyText.text = "Enemy : " + count;
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
        O_myCoinText.text = "My coin : " + GameManager.instance.Get_MyCoin();
    }

    // ���� Ŭ���� UI Ȱ��ȭ
    public void SetActiveGameClearUI(bool active)
    {
        gameclearUI.SetActive(active);
        C_myCoinText.text = "My coin : " + GameManager.instance.Get_MyCoin();
    }

    // ����Ʈ ���ھ� �ؽ�Ʈ ����
    public void UpdateBestScore(int cur_score)
    {
        O_bestScoreText.fontSize = 85;
        O_bestScoreText.text = "Best Score : " + cur_score;
        O_doteweenColor.GetComponent<DOTweenAnimation>().enabled = true;

        C_bestScoreText.fontSize = 85;
        C_bestScoreText.text = "Best Score : " + cur_score;
        C_bestScoreText.GetComponent<DOTweenAnimation>().enabled = true;
    }

    public void UnUpdateBestScore(int score)
    {
        O_bestScoreText.text = "Best Score : " + score;
        O_doteweenColor.GetComponent<DOTweenAnimation>().enabled = false;

        C_bestScoreText.text = "Best Score : " + score;
        C_bestScoreText.GetComponent<DOTweenAnimation>().enabled = false;
    }

    // ���� ȹ���� ���� ǥ��
    public void Cur_Score(int time)
    {
        int cscore = PlayerPrefs.GetInt("score1") + time;
        O_CurScoreText.text = "ȹ���� ���� : " + cscore;

        C_CurScoreText.text = "ȹ���� ���� : " + cscore;
    }
    // ���� ȹ���� ���� ǥ��
    public void Cur_Coin()
    {
        int ccoin = PlayerPrefs.GetInt("coin");
        O_CurCoinText.text = "ȹ���� ���� : " + ccoin;

        C_CurCoinText.text = "ȹ���� ���� : " + ccoin;
    }

    // ���� �����
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        OnTogglePause();
    }

    // ���� ����
    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // �� ��ȯ
    public void LoadScene(string sceneId)
    {
        SceneManager.LoadScene(sceneId);
        OnTogglePause();
    }

    // ȭ�� ���߰ų� �����̱�
    public void OnTogglePause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    // �޴�â Ȱ��ȭ
    public void OpenMenu()
    {
        Menu.SetActive(true);
        OnTogglePause();
    }
    // �޴�â ��Ȱ��ȭ
    public void CloseMenu()
    {
        Menu.SetActive(false);
        OnTogglePause();
    }

    // �ɼ�â Ȱ��ȭ
    public void OpenOption()
    {
        Option.SetActive(true);
    }

    // �ɼ�â ��Ȱ��ȭ
    public void CloseOption()
    {
        Option.SetActive(false);
    }

    // ����â Ȱ��ȭ
    public void OpenError()
    {
        ErrorPopUp.SetActive(true);
    }

    // ����â ��Ȱ��ȭ
    public void CloseError()
    {
        ErrorPopUp.SetActive(false);
    }

    // ���â Ȱ��ȭ
    public void OpenQuit()
    {
        QuitPopUp.SetActive(true);
    }

    // ���â ��Ȱ��ȭ
    public void CloseQuit()
    {
        QuitPopUp.SetActive(false);
    }

    // ������ ���� ��ư
    public void Delete_btn()
    {
        PlayerPrefs.DeleteAll();
    }

    // ������ ���� ���â Ȱ��ȭ
    public void DateResetPopUpOpen()
    {
        DateResetPopUp.SetActive(true);
    }

    // ������ ���� ���â ��Ȱ��ȭ
    public void DateResetPopUpClose()
    {
        DateResetPopUp.SetActive(false);
    }

    // ä��â Ȱ��ȭ
    public void ChatOpen()
    {
        ChatPopup.SetActive(true);
    }

    // ä��â ��Ȱ��ȭ
    public void ChatClose()
    {
        ChatPopup.SetActive(false);
    }

    // ��ŷâ Ȱ��ȭ
    public void RankingOpen()
    {
        RankingPopUp.SetActive(true);

        // ����� ���� 3���� ��ŷ ������ �ҷ��ͼ� UI�� ǥ��
        for (int i = 0; i < 3; i++)
        {
            string rankName = PlayerPrefs.GetString("rank_name" + i, "None");
            int rankScore = PlayerPrefs.GetInt("rank_score" + i, 0);

            if (i == 0)
            {
                rank1NameText.text = rankName;
                rank1ScoreText.text = "Score : " + rankScore;
            }
            else if (i == 1)
            {
                rank2NameText.text = rankName;
                rank2ScoreText.text = "Score : " + rankScore;
            }
            else if (i == 2)
            {
                rank3NameText.text = rankName;
                rank3ScoreText.text = "Score : " + rankScore;
            }
        }
    }

    // ��ŷâ ��Ȱ��ȭ
    public void RakingClose()
    {
        RankingPopUp.SetActive(false);
    }
    

    // �Ҹ� ��Ʈ�ϴ� ���
    public void BGMMuteToggle(bool isOn)
    {
        if(isOn)
        {
            SoundManager.instance.BGMSoundAllOn();
        }
        else
        {
            SoundManager.instance.BGMSoundAllMute();
        }
    }

    public void SFXMuteToggle(bool isOn)
    {
        if (isOn)
        {
            SoundManager.instance.SFXSoundAllOn();
        }
        else
        {
            SoundManager.instance.SFXSoundAllMute();
        }
    }

    public void AudioControl()
    {
        float sound1 = BGMSlider.value;
        float sound2 = SFXSlider.value;

        if(sound1 == -40f)
        {
            masterMixer.SetFloat("BGM", -80);
        }
        else
        {
            masterMixer.SetFloat("BGM", sound1);
        }

        if (sound2 == -40f)
        {
            masterMixer.SetFloat("SFX", -80);
        }
        else
        {
            masterMixer.SetFloat("SFX", sound2);
        }
    }
}