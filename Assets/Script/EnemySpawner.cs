using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyMiniPrefabs; // 생성할 원본 프리팹들
    public Enemy enemyMiddePrefab;
    public Enemy enemyBossPrefab;

    public EnemyData[] enemyData; // 사용할 셋업 데이터
    public Transform[] spawnMiniPoints; // AI를 소환할 위치들
    public Transform[] spawnMiddePoints;
    public Transform spawnBossPoint;

    private List<Enemy> enemys = new List<Enemy>(); // 생성된 적들을 담는 리스트

    private int wave; // 현재 웨이브

    private void Start()
    {
        wave = 0;
    }

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        if (wave >= 0 && wave < 5 && enemys.Count <= 0)
        {
            SpawnWave();
        }
        else if(wave == 5 && enemys.Count <= 0)
        {
            Debug.Log("ES_Update_waveClear");
            GameClear();
        }

        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI()
    {
        UIManager.instance.UpdateWaveText(wave);
        UIManager.instance.UpdateEnemyText(enemys.Count);
    }

    // 현재 웨이브에 맞춰 적들을 생성
    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1f);

        if(wave <= 4)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                CreateMiniEnemy();
            }
            if(wave == 3)
            {
                CreateMiddleEnemy();
            }
        }
        else if(wave == 5)
        {
            CreateBossEnemy();
        }
        
    }

    // 적 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateMiniEnemy()
    {
        Transform spawnPoint = spawnMiniPoints[Random.Range(0, spawnMiniPoints.Length)];

        Enemy enemy = Instantiate(enemyMiniPrefabs[Random.Range(0, enemyMiniPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(enemyData[0]);

        enemys.Add(enemy);

        enemy.onDeath += () => enemys.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 3f);
        enemy.onDeath += () => GameManager.instance.AddScore(10);
    }

    private void CreateMiddleEnemy()
    {
        Transform spawnPoint = spawnMiddePoints[Random.Range(0, spawnMiddePoints.Length)];

        Enemy enemy = Instantiate(enemyMiddePrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(enemyData[1]);


        enemys.Add(enemy);

        enemy.onDeath += () => enemys.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 3f);
        enemy.onDeath += () => GameManager.instance.AddScore(50);
    }

    private void CreateBossEnemy()
    {
        Transform spawnPoint = spawnBossPoint;
        Enemy enemy = Instantiate(enemyBossPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.Setup(enemyData[2]);


        enemys.Add(enemy);

        enemy.onDeath += () => enemys.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 3f);
        enemy.onDeath += () => GameManager.instance.AddScore(100);
    }

    private void GameClear()
    {
        GameManager.instance.ClearGame();
    }
}