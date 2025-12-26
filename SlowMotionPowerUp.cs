using UnityEngine;

public class SlowMotionPowerUp : PowerUp
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Range(0.1f, 1f)]
    public float slowMultiplier = 0.4f;

    protected override void Activate()
    {
        GameSpeedManager.Instance.ActivateSlowMotion(slowMultiplier, duration);
    }
}
