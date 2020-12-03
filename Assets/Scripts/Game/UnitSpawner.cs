using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject UnitPrefab;
    
    public int numberOfWaves;
    public int enemiesPerWave;
    public float spawnRate = 0.5f;
    public int secondsBetweenWaves;
    public int secondsStartDelay;
    public int pathId;

    private int _currentWave = 0;
    private WaypointManager.Path _path;

    public void Init(WaypointManager.Path path)
    {
        _path = path;
    }

    public void StartSpawner()
    {
        StartCoroutine("BeginWaveSpawn");
    }

    private IEnumerator BeginWaveSpawn()
    {
        yield return new WaitForSeconds(secondsStartDelay);

        while (_currentWave < numberOfWaves)
        {
            yield return SpawnWave(++_currentWave);
            yield return new WaitForSeconds(secondsBetweenWaves);
        }
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        ObjectPool_Manager poolManager = ServiceLocator.Get < ObjectPool_Manager>();
        for (int i = 0; i < enemiesPerWave; ++i)
        {
            GameObject enemy = poolManager.GetObjectFromPool("Enemies");            
            enemy.transform.position = transform.position;
            enemy.transform.rotation = transform.rotation;
            enemy.SetActive(true);
            
            // This needs integration
            //enemy.GetComponent<Enemy>().Initialize(_path);            
            
            yield return new WaitForSeconds(spawnRate);
        }        
    }
}
