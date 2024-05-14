using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int noOfEnemies;
    public int initialNoOfEnemies;  // Initial number of enemies to maintain the original count
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public Animator animator;
    public Text waveName;
    public Text waveInfoText;
    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;
    private bool canSpawn = true;
    private bool canAnimate = false;

    private void Start()
    {
        InitializeWaves();
        LoadWaveProgress();
    }

    private void Update()
    {
        if (waves == null || waves.Length == 0) return;

        currentWave = waves[currentWaveNumber];
        waveInfoText.text = "Wave: " + (currentWaveNumber + 1);

        SpawnWave();

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0 && currentWave.noOfEnemies == 0)
        {
            if (currentWaveNumber + 1 < waves.Length)
            {
                if (canAnimate)
                {
                    if (waves[currentWaveNumber + 1] != null)
                    {
                        waveName.text = waves[currentWaveNumber + 1].waveName;
                        animator.SetTrigger("WaveComplete");
                        canAnimate = false;
                    }
                }
            }
            else if (currentWaveNumber + 1 == waves.Length)
            {
                animator.SetTrigger("AllWavesComplete");
            }
        }
    }

    private void InitializeWaves()
    {
        foreach (var wave in waves)
        {
            if (wave.initialNoOfEnemies == 0)  // Only set initialNoOfEnemies if it is not already set
            {
                wave.initialNoOfEnemies = wave.noOfEnemies;
            }
        }
    }

    void SpawnNextWave()
    {
        if (currentWaveNumber + 1 < waves.Length)
        {
            currentWaveNumber++;
            canSpawn = true;
            SaveWaveProgress();
        }
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time && currentWave.noOfEnemies > 0)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            currentWave.noOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentWave.noOfEnemies == 0)
            {
                canSpawn = false;
                canAnimate = true;
            }
        }
    }

    public void PauseSpawning()
    {
       canSpawn = false;
    }

    public void ResumeSpawning()
    {
        canSpawn = true;
        if (nextSpawnTime < Time.time) {
            nextSpawnTime = Time.time + currentWave.spawnInterval;
        }
    }

    public void ResetWaveProgression()
    {
        currentWaveNumber = 0;
        canSpawn = true;
        canAnimate = false;
        nextSpawnTime = 0;
        InitializeWaves();  // Reset initial number of enemies correctly

        // Reset the number of enemies for each wave
        foreach (var wave in waves)
        {
            wave.noOfEnemies = wave.initialNoOfEnemies;
        }

        currentWave = waves[currentWaveNumber];
        waveName.text = currentWave.waveName;

        // Destroy all existing enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        PlayerPrefs.DeleteKey("CurrentWaveNumber");
        PlayerPrefs.Save();
    }

    private void SaveWaveProgress()
    {
        PlayerPrefs.SetInt("CurrentWaveNumber", currentWaveNumber);
        PlayerPrefs.Save();
    }

    private void LoadWaveProgress()
    {
        currentWaveNumber = PlayerPrefs.GetInt("CurrentWaveNumber", 0);
        if (currentWaveNumber >= waves.Length) currentWaveNumber = 0;
        currentWave = waves[currentWaveNumber];
        waveName.text = currentWave.waveName;
    }
}
