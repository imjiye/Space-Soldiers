using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

// ������ ���� ���� ���θ� �����ϴ� ���� �Ŵ���
public class GameManager : MonoBehaviour
{
    // �̱��� ���ٿ� ������Ƽ
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

    private int score = 0; // ���� ���� ����
    private int coin = 0; // ���� ���� ����

    float surviveTime;

    public GameObject[] players;

    public InputField InputTextName_O; // �÷��̾� �г��� �Է� UI
    public InputField InputTextName_C;

    public bool isGameover { get; private set; } // ���� ���� ����

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        // �÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
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

    // ������ �߰��ϰ� UI ����
    public void AddScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            score += newScore;
            // ���� ���� ����
            PlayerPrefs.SetInt("score1", score);
            // ���� UI �ؽ�Ʈ ����
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // ������ �߰��ϰ� UI ����
    public void AddCoin(int newCoin)
    {
        if(!isGameover)
        {
            // ���� �߰�
            coin += newCoin;
            // ���� ���� ����
            PlayerPrefs.SetInt("coin", coin);
            // ���� UI �ؽ�Ʈ ����
            UIManager.instance.UpdateCoinText(coin);
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        isGameover = true;

        //int b = Get_1BestScore() + (int)surviveTime;
        // ���� ȹ�� ����
        int baseScore = PlayerPrefs.GetInt("score1");

        // ���� �ð��� �������� ���� ������ �ް� ��
        int survivalScore = (int)(1000 / (surviveTime + 1)); // ���� �ð��� �������� ū ���� ����
        int finalScore = baseScore + survivalScore;

        // ���� ������ �ְ� ������ ����
        Set_1BestScore(finalScore);

        //int s = PlayerPrefs.GetInt("score1") + (int)surviveTime;
        //Set_1BestScore(s);

        //int mc = Get_MyCoin();
        Set_MyCoin(coin);

        // �г����� �Է¹޾� �����ϱ�
        string username = InputTextName_O.text; 

        UIManager.instance.SetActiveGameoverUI(true);
        UIManager.instance.Cur_Score((int)surviveTime);
        UIManager.instance.Cur_Coin();

        // ��ŷ ������Ʈ �Լ� ȣ��
        UpdateRanking(finalScore, username);

        //SoundManager.instance.PlaySound("end");
    }
    
    // ���� Ŭ���� ó��
    public void ClearGame()
    {
        Debug.Log("GM_ClearGame");
        isGameover = true;

        //int b = Get_1BestScore() + (int)surviveTime;
        //int s = PlayerPrefs.GetInt("score1") + (int)surviveTime;
        //Set_1BestScore(s);

        // ���� ȹ�� ����
        int baseScore = PlayerPrefs.GetInt("score1");

        // ���� �ð��� �������� ���� ������ �ް� ��
        int survivalScore = (int)(1000 / (surviveTime + 1)); // ���� �ð��� �������� ū ���� ����
        int finalScore = baseScore + survivalScore;

        // ���� ������ �ְ� ������ ����
        Set_1BestScore(finalScore);

        //int mc = Get_MyCoin();
        Set_MyCoin(coin);

        // �г����� �Է¹޾� �����ϱ�
        string username = InputTextName_C.text;

        UIManager.instance.SetActiveGameClearUI(true);
        UIManager.instance.Cur_Score((int)surviveTime);
        UIManager.instance.Cur_Coin();

        // ��ŷ ������Ʈ �Լ� ȣ��
        UpdateRanking(finalScore, username);

        //SoundManager.instance.PlaySound("Clear");
    }

    public void UpdateRanking(int score, string username)
    {
        // ���� ����� ���� 3���� ������ �г����� �ҷ���
        int[] topScores = new int[3];
        string[] topNames = new string[3];

        for (int i = 0; i < 3; i++)
        {
            topScores[i] = PlayerPrefs.GetInt("rank_score" + i, 0);
            topNames[i] = PlayerPrefs.GetString("rank_name" + i, "None");
        }

        // ���� ������ ���� 3�� �ȿ� �� �� �ִ��� üũ
        for (int i = 0; i < 3; i++)
        {
            if (score > topScores[i])
            {
                // ���� ���� ������ ������ ��� ���� ������ �� ĭ�� ����
                for (int j = 2; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                    topNames[j] = topNames[j - 1];
                }
                // ���ο� ������ �ش� ������ ����
                topScores[i] = score;
                topNames[i] = username;
                break;
            }
        }

        // PlayerPrefs�� ������Ʈ�� ��ŷ ����
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("rank_score" + i, topScores[i]);
            PlayerPrefs.SetString("rank_name" + i, topNames[i]);
        }
    }


    // �ְ� ���� �����ϱ�
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

    // ���� �����ϱ�
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