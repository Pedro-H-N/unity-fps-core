using System.Collections;
using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour
{
    [Header("References")]
    public Camera fpsCamera;
    public Transform muzzlePoint;
    public GameObject tracerPrefab;

    [Header("Damage")]
    public float range = 50f;
    public float damage = 10f;

    [Header("Shot Pattern")]
    public int pellets = 1;
    public float spread = 0f;

    [Header("Fire Settings")]
    public float fireRate = 0.3f;
    public FireMode fireMode = FireMode.Automatic;

    [Header("Reload")]
    public float reloadTime;
    public float maxAmmo;
    public float currentAmmo;
    public bool isReloading;


    [Header("Audio")]
    public AudioClip gunShotClip;
    public AudioClip reloadClip;
    public float shotSoundDelay = 0f;

    [Header("Empty Ammo")]
    public AudioClip emptyAmmoClip;
    public float emptyAmmoCooldown = 0.3f;

    private float nextEmptySoundTime;

    private AudioSource audioSource;
    private float nextFireTime;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (fpsCamera == null)
            fpsCamera = Camera.main;
    }

    void Update()
    {
        HandleInput();
        ReloadCoroutine();
    }

    void HandleInput()
    {
        if(isReloading)
        return;

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadCoroutine());
            return;
        }

        if (Time.time < nextFireTime)
            return;

        switch (fireMode)
        {
            case FireMode.Automatic:
                if (Input.GetMouseButton(0))
                    Fire();
                break;

            case FireMode.SemiAutomatic:
                if (Input.GetMouseButtonDown(0))
                    Fire();
                break;
        }
    }

    void Fire()
    {
        if (isReloading)
        return;

        if(currentAmmo <= 0)
        {
            PlayEmptyAmmoSound();
            return;    
        }

        Shoot();
        StartCoroutine(PlayShotSound());

        float finalFireRate = fireRate * GameSpeedManager.Instance.playerFireRateMultiplier;
        nextFireTime = Time.time + finalFireRate;
    }

    void Shoot()
    {
        if(currentAmmo == 0)
        return;

        for (int i = 0; i < pellets; i++)
        {
            Vector3 direction = GetShotDirection();
            Vector3 origin = fpsCamera.transform.position;
            Vector3 hitPoint = origin + direction * range;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
            {
                hitPoint = hit.point;

                if (hit.collider.TryGetComponent(out Enemy enemy))
                    enemy.TakeDamage(damage);
            }

            SpawnTracer(hitPoint);
        }
        currentAmmo --;
    }

    IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        audioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    Vector3 GetShotDirection()
    {
        Vector3 direction = fpsCamera.transform.forward;

        if (spread > 0f)
        {
            direction = Quaternion.Euler(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                0f
            ) * direction;
        }

        return direction;
    }

    void SpawnTracer(Vector3 hitPoint)
    {
        BulletTracer tracer = Instantiate(tracerPrefab)
            .GetComponent<BulletTracer>();

        tracer.Draw(muzzlePoint.position, hitPoint);
    }

    IEnumerator PlayShotSound()
    {
        if (shotSoundDelay > 0f)
            yield return new WaitForSeconds(shotSoundDelay);

        audioSource.PlayOneShot(gunShotClip);
    }

    void PlayEmptyAmmoSound()
    {   
        if (Time.time < nextEmptySoundTime)
        return;

        audioSource.PlayOneShot(emptyAmmoClip);
        nextEmptySoundTime = Time.time + emptyAmmoCooldown;
    }

    public enum FireMode
    {
        Automatic,
        SemiAutomatic
    }
}
