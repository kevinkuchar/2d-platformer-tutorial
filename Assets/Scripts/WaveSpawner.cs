using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string name;
    public Transform enemy;
    public float rate;
    public int count;
}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    
    private SpawnState state = SpawnState.COUNTING;
    private float waveCountdown;
    private float searchCountdown = 1f;
    private int nextWave = 0;

    public SpawnState State {
        get { return state; }
    }

    public float WaveCountDown {
        get { return waveCountdown + 1; }
    }

    public int NextWave {
        get { return nextWave + 1; }
    }

    void Start () {
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0 ) {
            Debug.Log("No spawn points referenced.");
        }
        if (waves.Length == 0) {
            Debug.Log("No waves referenced.");
        }
    }

    void Update () {
        if (state == SpawnState.WAITING) {
           if (EnemyIsAlive () == false) {
               WaveCompleted ();
           } else {
               return;
           }
        }

        if (waveCountdown <= 0) {
            if (state != SpawnState.SPAWNING) {
                StartCoroutine (SpawnWave (waves[nextWave]));
            }
        } else {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted () {
        Debug.Log("Wave Completed!");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length -1) {
            nextWave = 0;
            Debug.Log("Completed all waves. Looping.");
        } else {
            nextWave++;
        }
    }

    bool EnemyIsAlive () {
        searchCountdown -= Time.deltaTime;
        
        if (searchCountdown <= 0) {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave (Wave _wave) {
        Debug.Log("Spawning Wave");
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++) {
            SpawnEnemy (_wave.enemy);
            yield return new WaitForSeconds ( 1f/_wave.rate );
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy (Transform _enemy) {
        Debug.Log("Spawning Enemy" + _enemy.name);
        Transform _sp = spawnPoints[ Random.Range(0, spawnPoints.Length) ];
        Instantiate (_enemy, _sp.position, _sp.rotation);
    }
}
