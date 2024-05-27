using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [HideInInspector] public float duration = 0.8f;

    // Functie die wordt aangeroepen wanneer een andere collider het triggergebied binnenkomt
    void OnTriggerEnter2D(Collider2D col)
    {
        // Controleer of de object de tag "Player" heeft
        if (col.gameObject.CompareTag("Player"))
        {
            GameControl.moneyAmount += 5; // Verhoog het geldbedrag met 5
            PlayerPrefs.SetInt("MoneyAmount", GameControl.moneyAmount); // Sla het nieuwe geldbedrag op
            GameControl.Instance.UpdateMoneyDisplay(); // Update de weergave van het geldbedrag
            StartCoroutine(AnimateItemPickup()); // Start de animatie coroutine

            // Verhoog de statistiek voor verzameld geld
            StatisticsManager.Instance.IncrementMoneyCollected(5);
        }
    }

    // Coroutine om de animatie van het oppakken van een item te beheren
    private IEnumerator AnimateItemPickup()
    {
        Vector3 startScale = transform.localScale; // Begin schaal van de munt
        Vector3 endScale = Vector3.zero; // Eind schaal van de munt (verdwijnen)
        float currentTime = 0; // Huidige tijd van de animatie

        // Voer de animatie uit totdat de huidige tijd de duur bereikt
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime; // Verhoog de huidige tijd met de tijd die is verstreken sinds de laatste frame
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration); // Interpoleer de schaal van start naar eind
            yield return null; // Wacht tot de volgende frame
        }

        Destroy(gameObject); // Vernietig de munt nadat de animatie is voltooid
    }
}

// Interpolatie is het afleiden van nieuwe datapunten binnen het bereik van een verzameling bekende discrete datapunten onder de veronderstelling van een zekere relatie tussen die punten