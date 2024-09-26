using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private static GameManager m_instance;

    private int score = 0; // 현재 게임 점수
    private int coin = 0; // 현재 얻은 코인

    float surviveTime;

    public GameObject[] players;

    public InputField InputTextName_O; // 플레이어 닉네임 입력 UI
    public InputField InputTextName_C;

    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        for (int i = 0; i < players.Length; i++)
        {
            if ((int)DataManager.instance.CurCharcter == i)
            {
                players[i].GetComponent<PlayerHealth>().onDeath += EndGame;
            }
        }
    }

    private void Start()
    {
        surviveTime = 0;
    }

    private void Update()
    {
        if(!isGameover)
        {
            surviveTime += Time.deltaTime;
            UIManager.instance.TimeUpdate(surviveTime);
        }
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 정보 저장
            PlayerPrefs.SetInt("score1", score);
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 코인을 추가하고 UI 갱신
    public void AddCoin(int newCoin)
    {
        if(!isGameover)
        {
            // 코인 추가
            coin += newCoin;
            // 코인 정보 저장
            PlayerPrefs.SetInt("coin", coin);
            // 코인 UI 텍스트 갱신
            UIManager.instance.UpdateCoinText(coin);
        }
    }

    // 게임 오버 처리
    public void EndGame()
    {
        isGameover = true;

        //int b = Get_1BestScore() + (int)surviveTime;
        // 기존 획득 점수
        int baseScore = PlayerPrefs.GetInt("score1");

        // 생존 시간이 적을수록 높은 점수를 받게 함
        int survivalScore = (int)(1000 / (surviveTime + 1)); // 생존 시간이 적을수록 큰 값이 나옴
        int finalScore = baseScore + survivalScore;

        // 최종 점수를 최고 점수로 저장
        Set_1BestScore(finalScore);

        //int s = PlayerPrefs.GetInt("score1") + (int)surviveTime;
        //Set_1BestScore(s);

        //int mc = Get_MyCoin();
        Set_MyCoin(coin);

        // 닉네임을 입력받아 저장하기
        string username = InputTextName_O.text; 

        UIManager.instance.SetActiveGameoverUI(true);
        UIManager.instance.Cur_Score((int)surviveTime);
        UIManager.instance.Cur_Coin();

        // 랭킹 업데이트 함수 호출
        UpdateRanking(finalScore, username);

        //SoundManager.instance.PlaySound("end");
    }
    
    // 게임 클리어 처리
    public void ClearGame()
    {
        Debug.Log("GM_ClearGame");
        isGameover = true;

        //int b = Get_1BestScore() + (int)surviveTime;
        //int s = PlayerPrefs.GetInt("score1") + (int)surviveTime;
        //Set_1BestScore(s);

        // 기존 획득 점수
        int baseScore = PlayerPrefs.GetInt("score1");

        // 생존 시간이 적을수록 높은 점수를 받게 함
        int survivalScore = (int)(1000 / (surviveTime + 1)); // 생존 시간이 적을수록 큰 값이 나옴
        int finalScore = baseScore + survivalScore;

        // 최종 점수를 최고 점수로 저장
        Set_1BestScore(finalScore);

        //int mc = Get_MyCoin();
        Set_MyCoin(coin);

        // 닉네임을 입력받아 저장하기
        string username = InputTextName_C.text;

        UIManager.instance.SetActiveGameClearUI(true);
        UIManager.instance.Cur_Score((int)surviveTime);
        UIManager.instance.Cur_Coin();

        // 랭킹 업데이트 함수 호출
        UpdateRanking(finalScore, username);

        //SoundManager.instance.PlaySound("Clear");
    }

    public void UpdateRanking(int score, string username)
    {
        // 현재 저장된 상위 3명의 점수와 닉네임을 불러옴
        int[] topScores = new int[3];
        string[] topNames = new string[3];

        for (int i = 0; i < 3; i++)
        {
            topScores[i] = PlayerPrefs.GetInt("rank_score" + i, 0);
            topNames[i] = PlayerPrefs.GetString("rank_name" + i, "None");
        }

        // 현재 점수가 상위 3위 안에 들어갈 수 있는지 체크
        for (int i = 0; i < 3; i++)
        {
            if (score > topScores[i])
            {
                // 새로 들어온 점수가 순위에 들면 기존 순위를 한 칸씩 내림
                for (int j = 2; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                    topNames[j] = topNames[j - 1];
                }
                // 새로운 점수를 해당 순위에 삽입
                topScores[i] = score;
                topNames[i] = username;
                break;
            }
        }

        // PlayerPrefs에 업데이트된 랭킹 저장
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("rank_score" + i, topScores[i]);
            PlayerPrefs.SetString("rank_name" + i, topNames[i]);
        }
    }


    // 최고 점수 저장하기
    public int Get_1BestScore()
    {
        int BS = PlayerPrefs.GetInt("1BestScore");
        return BS;
    }

    public void Set_1BestScore(int cur_score)
    {
        if (cur_score > Get_1BestScore())
        {
            PlayerPrefs.SetInt("1BestScore", cur_score);
            UIManager.instance.UpdateBestScore(cur_score);
        }
        else
        {
            UIManager.instance.UnUpdateBestScore(Get_1BestScore());
        }
    }

    // 코인 누적하기
    public int Get_MyCoin()
    {
        int myc = PlayerPrefs.GetInt("MyCoin");
        return myc;
    }

    public void Set_MyCoin(int Haning_Coin)
    {
        PlayerPrefs.SetInt("MyCoin", Haning_Coin + Get_MyCoin());
    }
}