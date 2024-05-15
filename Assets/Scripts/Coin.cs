using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [HideInInspector] public float duration = 0.8f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameControl.moneyAmount += 5;
            PlayerPrefs.SetInt("MoneyAmount", GameControl.moneyAmount);
            GameControl.Instance.UpdateMoneyDisplay();
            StartCoroutine(AnimateItemPickup());

            StatisticsManager.Instance.IncrementMoneyCollected(5);
        }
    }

    private IEnumerator AnimateItemPickup()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}