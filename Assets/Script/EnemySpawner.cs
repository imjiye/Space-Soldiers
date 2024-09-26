using System.Collections.Generic;
using UnityEngine;

// �� ���� ������Ʈ�� �ֱ������� ����
public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyMiniPrefabs; // ������ ���� �����յ�
    public Enemy enemyMiddePrefab;
    public Enemy enemyBossPrefab;

    public EnemyData[] enemyData; // ����� �¾� ������
    public Transform[] spawnMiniPoints; // AI�� ��ȯ�� ��ġ��
    public Transform[] spawnMiddePoints;
    public Transform spawnBossPoint;

    private List<Enemy> enemys = new List<Enemy>(); // ������ ������ ��� ����Ʈ

    private int wave; // ���� ���̺�

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

    // ���̺� ������ UI�� ǥ��
    private void UpdateUI()
    {
        UIManager.instance.UpdateWaveText(wave);
        UIManager.instance.UpdateEnemyText(enemys.Count);
    }

    // ���� ���̺꿡 ���� ������ ����
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

    // �� �����ϰ� ������ ������ ������ ����� �Ҵ�
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