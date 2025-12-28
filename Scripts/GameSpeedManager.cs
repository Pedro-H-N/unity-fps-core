using System.Collections;
using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    public static GameSpeedManager Instance;

    [Header("Enemy")]
    public float enemySpeedMultiplier = 1f;
    public float enemyFireRateMultiplier = 1f;
    public float enemyProjectileSpeedMultiplier = 1f;

    [Header("Player")]
    public float playerFireRateMultiplier = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // -------- POWER UPS --------

    public void ActivateSlowMotion(float slowMultiplier, float duration)
    {
        StartCoroutine(SlowMotionCoroutine(slowMultiplier, duration));
    }

    IEnumerator SlowMotionCoroutine(float slowMultiplier, float duration)
    {
        enemySpeedMultiplier = slowMultiplier;
        enemyFireRateMultiplier = slowMultiplier;
        enemyProjectileSpeedMultiplier = slowMultiplier;

        yield return new WaitForSeconds(duration);

        enemySpeedMultiplier = 1f;
        enemyFireRateMultiplier = 1f;
        enemyProjectileSpeedMultiplier = 1f;
    }

    public void ActivateFireRateBoost(float boostMultiplier, float duration)
    {
        StartCoroutine(FireRateBoostCoroutine(boostMultiplier, duration));
    }

    IEnumerator FireRateBoostCoroutine(float boostMultiplier, float duration)
    {
        playerFireRateMultiplier = boostMultiplier;
        yield return new WaitForSeconds(duration);
        playerFireRateMultiplier = 1f;
    }
}