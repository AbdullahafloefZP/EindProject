using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public static event EventHandler OnEnemyDeath;
    [SerializeField] private float health, maxHealth = 3f;
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private GameObject coinPrefab;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private Coroutine damageFlashCoroutine;
    [HideInInspector] public CoinReward printCoin;

    private void Start() 
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount) 
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            OnEnemyDeath?.Invoke(this, EventArgs.Empty);
            
            StatisticsManager.Instance.IncrementEnemiesKilled();
        }
    }

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;   
        }
    }

    public void CallDamageFlash() 
    {
        damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher() 
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);


            yield return null;
        }


    }

    private void SetFlashColor() 
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);   
        }
    }

    private void SetFlashAmount(float amount) 
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}
