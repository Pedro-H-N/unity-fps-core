using UnityEngine;

public class FireRatePowerUp : PowerUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float fireRateMultiplier = 0.5f;

    protected override void Activate()
    {
        GameSpeedManager.Instance.ActivateFireRateBoost(fireRateMultiplier, duration);
    }
}
