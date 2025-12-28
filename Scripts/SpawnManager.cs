using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnDelay = 1.0f;
    public GameObject[] enemyPrefabs;
    public GameObject[] powerUps;
    public GameObject ShotgunPrefab;

    public float spawnLimit = 24.0f;
    public int enemiesAlive;
    public bool isGameActive;
    public bool isSpawningWave;
    public int waveNumber = 1;
    public int shotgunWave = 3;






    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameActive = true;
        SpawnEnemyWave(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive && enemiesAlive == 0 && !isSpawningWave)
        {
            StartCoroutine(NextWave());
        }
    }


    Vector3 GenerateSpawnPos()
    {
        float x = Random.Range(-spawnLimit, spawnLimit);
        float z = Random.Range(-spawnLimit, spawnLimit);

        return new Vector3(x, 1, z);
    }

    void SpawnEnemyWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[index], GenerateSpawnPos(), enemyPrefabs[index].transform.rotation);
            enemiesAlive++;
        }
        
        TrySpawnPowerUp();
    }


    IEnumerator NextWave()
    {
        isSpawningWave = true;

        yield return new WaitForSeconds(spawnDelay);

        waveNumber++;
        SpawnEnemyWave(waveNumber);
        if (waveNumber == 3)
        {
            SpawnShotgun();
        }

        isSpawningWave = false;
    }


    void SpawnShotgun()
    {
        Vector3 pos = GenerateSpawnPos();
        Instantiate(ShotgunPrefab, pos, Quaternion.identity);
    }

    void TrySpawnPowerUp()
    {

        if (waveNumber % 2 == 0) // a cada 2 waves
        {
            int index = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[index], GenerateSpawnPos(), Quaternion.identity);
        }
    }   
}
