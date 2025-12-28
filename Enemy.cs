using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    public float hp = 3f;
    public float speed = 5f;
    public float retreatRange = 4f;
    public float attackRange = 10f;
    private float nextFireTime;
    public float fireRate = 1.5f;

    [Header("Objects")]
    public GameObject projectilePrefab;
    public Transform shootPoint;

    [Header("Mentions")]
    
    private GameObject player;
    private SpawnManager spawnManager;
    private Renderer enemyRenderer;
    private Color originalColor;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        spawnManager = FindAnyObjectByType<SpawnManager>();

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    void Update()
    {
        if (!player) return;

        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0f;

        float distance = directionToPlayer.magnitude;
        Vector3 dir = directionToPlayer.normalized;

        float finalSpeed = speed * GameSpeedManager.Instance.enemySpeedMultiplier;

        if (distance > attackRange)
        {
            transform.position += dir * finalSpeed * Time.deltaTime;
        }
        else if (distance < retreatRange)
        {
            transform.position -= dir * finalSpeed * Time.deltaTime;
        }
        else
        {
            TryShoot();
        }

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime) return;

        float fireRateMultiplier = GameSpeedManager.Instance.enemyFireRateMultiplier;

        float finalFireRate = fireRate / fireRateMultiplier;
        nextFireTime = Time.time + finalFireRate;

        GameObject bullet = Instantiate(
            projectilePrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = (player.transform.position - shootPoint.position).normalized;

        float projectileSpeed = 15f * GameSpeedManager.Instance.enemyProjectileSpeedMultiplier;
        rb.AddForce(dir * projectileSpeed, ForceMode.Impulse);
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        StartCoroutine(FlashRed());

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (spawnManager)
            spawnManager.enemiesAlive--;
    }

    IEnumerator FlashRed()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material.color = originalColor;
    }
}


