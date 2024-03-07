using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private float swarmerInterval = 3.5f;
    [SerializeField] private float lineOfSight;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }

    void Awake()
    {
        StartCoroutine(SpawnEnemiesWithInterval(swarmerInterval, swarmerPrefab));
    }

    private IEnumerator SpawnEnemiesWithInterval(float interval, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitUntil(() => IsPlayerInLineOfSight());
            Vector3 randomPosition = GetRandomPositionInLineOfSight();
            Instantiate(enemy, randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }

    private bool IsPlayerInLineOfSight()
    {
        Vector3 playerPosition = PlayerPosition();
        return Vector3.Distance(transform.position, playerPosition) <= lineOfSight;
    }

    private Vector3 PlayerPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            return player.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 GetRandomPositionInLineOfSight()
    {
        Vector3 center = transform.position;
        float radius = lineOfSight;

        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        randomPoint.z = 0;

        return randomPoint;
    }
}
