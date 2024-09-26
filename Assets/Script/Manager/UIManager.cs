using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using UnityEngine.Audio;

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
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

    public GameObject Menu; // 메뉴 버튼 클릭시 활성화될 메뉴창
    public GameObject Option; // 옵션 버튼 클릭시 활성화될 옵션창
    public GameObject QuitPopUp; // 게임 종료버튼 클릭시 활성화될 경고창

    public Text ammoText; // 탄약 표시
    public Text scoreText; // 점수 표시
    public Text coinText; // 코인 표시
    public Text waveText; // 적 웨이브 표시
    public Text enemyText; // 남아있는 적 표시
    public Text timeText; // 시간 표시

    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI
    public GameObject gameclearUI; // 게임 클리어시 활성화 할 UI 
    public GameObject ErrorPopUp; // 에러 팝업창 UI
    public GameObject DateResetPopUp; // 데이터 삭제 경고 창 UI
    public GameObject ChatPopup; // 채팅창 UI

    public GameObject RankingPopUp; // 랭킹 팝업 UI
    public Text rank1NameText;
    public Text rank1ScoreText;
    public Text rank2NameText;
    public Text rank2ScoreText;
    public Text rank3NameText;
    public Text rank3ScoreText;

    // 베스트 스코어 표시
    public Text O_bestScoreText; 
    public Text C_bestScoreText;
    // 획득한 점수 표시
    public Text O_CurScoreText; 
    public Text C_CurScoreText;
    // 획득한 코인 표시
    public Text O_CurCoinText;
    public Text C_CurCoinText;
    // 보유한 코인 표시
    public Text O_myCoinText;
    public Text C_myCoinText;
    // DOTeween을 이용한 베스트스코어 색상 변환
    public GameObject O_doteweenColor; 
    public GameObject C_doteweenColor;

    public Text HaveCoin; // 현재 누적으로 가진 코인 표시

    public AudioMixer masterMixer; // 오디오 조절을 위한 믹서
    public Slider BGMSlider; // BGM 조절 슬라이더
    public Slider SFXSlider; // 효과음 조절 슬라이더

    // 시간 지나는 거 보여주기
    public void TimeUpdate(float surviveTime)
    {
        timeText.text = "Time : " + (int) surviveTime;
    }

    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    // 코인 텍스트 갱신
    public void UpdateCoinText(int newCoin)
    {
        coinText.text = " " + newCoin;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves)
    {
        waveText.text = "Wave : " + waves;
    }

    // 남은 적 숫자 갱신
    public void UpdateEnemyText(int count)
    {
        enemyText.text = "Enemy : " + count;
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
        O_myCoinText.text = "My coin : " + GameManager.instance.Get_MyCoin();
    }

    // 게임 클리어 UI 활성화
    public void SetActiveGameClearUI(bool active)
    {
        gameclearUI.SetActive(active);
        C_myCoinText.text = "My coin : " + GameManager.instance.Get_MyCoin();
    }

    // 베스트 스코어 텍스트 변경
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

    // 현재 획득한 점수 표시
    public void Cur_Score(int time)
    {
        int cscore = PlayerPrefs.GetInt("score1") + time;
        O_CurScoreText.text = "획득한 점수 : " + cscore;

        C_CurScoreText.text = "획득한 점수 : " + cscore;
    }
    // 현재 획득한 코인 표시
    public void Cur_Coin()
    {
        int ccoin = PlayerPrefs.GetInt("coin");
        O_CurCoinText.text = "획득한 코인 : " + ccoin;

        C_CurCoinText.text = "획득한 코인 : " + ccoin;
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        OnTogglePause();
    }

    // 게임 종료
    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 씬 전환
    public void LoadScene(string sceneId)
    {
        SceneManager.LoadScene(sceneId);
        OnTogglePause();
    }

    // 화면 멈추거나 움직이기
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

    // 메뉴창 활성화
    public void OpenMenu()
    {
        Menu.SetActive(true);
        OnTogglePause();
    }
    // 메뉴창 비활성화
    public void CloseMenu()
    {
        Menu.SetActive(false);
        OnTogglePause();
    }

    // 옵션창 활성화
    public void OpenOption()
    {
        Option.SetActive(true);
    }

    // 옵션창 비활성화
    public void CloseOption()
    {
        Option.SetActive(false);
    }

    // 에러창 활성화
    public void OpenError()
    {
        ErrorPopUp.SetActive(true);
    }

    // 에러창 비활성화
    public void CloseError()
    {
        ErrorPopUp.SetActive(false);
    }

    // 경고창 활성화
    public void OpenQuit()
    {
        QuitPopUp.SetActive(true);
    }

    // 경고창 비활성화
    public void CloseQuit()
    {
        QuitPopUp.SetActive(false);
    }

    // 데이터 삭제 버튼
    public void Delete_btn()
    {
        PlayerPrefs.DeleteAll();
    }

    // 데이터 삭제 경고창 활성화
    public void DateResetPopUpOpen()
    {
        DateResetPopUp.SetActive(true);
    }

    // 데이터 삭제 경고창 비활성화
    public void DateResetPopUpClose()
    {
        DateResetPopUp.SetActive(false);
    }

    // 채팅창 활성화
    public void ChatOpen()
    {
        ChatPopup.SetActive(true);
    }

    // 채팅창 비활성화
    public void ChatClose()
    {
        ChatPopup.SetActive(false);
    }

    // 랭킹창 활성화
    public void RankingOpen()
    {
        RankingPopUp.SetActive(true);

        // 저장된 상위 3명의 랭킹 정보를 불러와서 UI에 표시
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

    // 랭킹창 비활성화
    public void RakingClose()
    {
        RankingPopUp.SetActive(false);
    }
    

    // 소리 뮤트하는 토글
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