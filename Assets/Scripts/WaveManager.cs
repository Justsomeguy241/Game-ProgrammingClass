using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs; // Assign all enemy types in Inspector
    public Transform[] spawnPoints;   // Where enemies can appear

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int baseEnemyCount = 5;
    public int baseBatchSize = 2;
    public float spawnDelay = 5f; // Time between batches
    public float interWaveDelay = 5f;

    private int totalEnemiesThisWave;
    private int enemiesSpawned = 0;
    private int batchSize;
   [SerializeField] private int enemiesAlive = 0;
    private bool isSpawning = false;

    [Header("UI")]
    public Text waveText; // Assign this in the Inspector


    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        // Check if wave is finished
        if (!isSpawning && enemiesAlive <= 0)
        {
            HandleWaveComplete();
        }
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;
        enemiesSpawned = 0;
        enemiesAlive = 0; // Reset here
        totalEnemiesThisWave = baseEnemyCount + currentWave * 2;
        batchSize = baseBatchSize + currentWave / 2;

        if (waveText != null)
            waveText.text = $"Wave {currentWave}";

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(SpawnWave());
    }


    IEnumerator SpawnWave()
    {
        while (enemiesSpawned < totalEnemiesThisWave)
        {
            int spawnCount = Mathf.Min(batchSize, totalEnemiesThisWave - enemiesSpawned);

            for (int i = 0; i < spawnCount; i++)
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                enemiesAlive++; // Increment when actually spawning
                enemy.GetComponent<EnemyScript>().OnDeath += OnEnemyDeath;
            }

            enemiesSpawned += spawnCount;
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }


    void OnEnemyDeath()
    {
        enemiesAlive--;
    }

    private void HandleWaveComplete()
    { 

        currentWave++;
        StartCoroutine(StartNextWave());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();  // Stops all running wave-related coroutines
        isSpawning = false;
    }

}
