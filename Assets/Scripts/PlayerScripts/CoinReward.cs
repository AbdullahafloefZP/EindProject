using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class CoinReward : MonoBehaviour
{
    public Text coinText;
    [HideInInspector] public int coinsEarned = 0;
    [HideInInspector] public int xpEarned = 0;
    [HideInInspector] public DamageFlash zombieDeath;

    public void AwardRewards()
    {
        coinsEarned += 2;
        xpEarned += 120;
    }

    public void UpdateCoinText()
    {
        coinText.text = "Coins: " + $"{coinsEarned}";
    }
}
