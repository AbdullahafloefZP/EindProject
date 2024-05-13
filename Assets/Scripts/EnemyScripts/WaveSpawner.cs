using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int noOfEnemies;
    public int initialNoOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public Animator animator;
    public Text waveName;
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
        currentWave = waves[currentWaveNumber];

        SpawnWave();

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0)
        {
            if (currentWaveNumber + 1 < waves.Length)
            {
                if (canAnimate)
                {
                    waveName.text = waves[currentWaveNumber + 1].waveName;
                    animator.SetTrigger("WaveComplete");
                    canAnimate = false;
                }
            }
        }
    }

    private void InitializeWaves()
    {
        foreach (var wave in waves)
        {
            wave.initialNoOfEnemies = wave.noOfEnemies;
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
        InitializeWaves();
        canSpawn = true;
        canAnimate = false;
        nextSpawnTime = 0;
        currentWave = waves[currentWaveNumber];
        waveName.text = currentWave.waveName;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
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
