using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    //public Transform enemy1;
    //public Transform enemy2;

    public enum SpawnState
    {
        SPAWNING, WAITING, COUNTING
    };
    public static string _state;

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public Transform enemyType1;
        public Transform enemyType2;
        public int count;
        public float spawnRate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    private int wavesCompleted = 0;
    public int NextWave
    {
        get { return nextWave + 1; } //so game doesn't start at level 0
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown + 1; } //so user can actually see first number. 5f = sees 5
    }
    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }

    AudioManager audioManager;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0) Debug.LogError("No spawnpoints linked in array.");
        if (waves.Length == 0) Debug.LogError("No Waves found.");

        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError("No AudioManager found.");
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            try
            {
                audioManager.StopSound("Battle");
                audioManager.StopSound("Intro");
            }
            catch { Debug.Log("Battle sound is not playing."); }

            if (!EnemyIsAlive())
            {
                audioManager.PlaySound("Intro");
                WaveCompleted();
            }
            else return; //if enemies are alive, you don't need all the stuff below.
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }                          
    void WaveCompleted()
    {
        nextWave++;
        if (nextWave >= waves.Length)
        {
            nextWave = 0;
            Debug.Log("All waves complete. Looping levels.");
        }

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) return false;
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {

        audioManager.PlaySound("Shop");
        try
        {
            audioManager.StopSound("Battle");
            audioManager.StopSound("Intro");

        }
        catch {};
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            float rand = Random.value;
            if (rand <= .5f) SpawnEnemy(_wave.enemyType1);
            else SpawnEnemy(_wave.enemyType2);
            yield return new WaitForSeconds(1f / _wave.spawnRate); //whenever you want to wait foo seconds
        }

        state = SpawnState.WAITING;
        yield break; //ends IEnumerator
    }

    Transform _lastSP;
    void SpawnEnemy(Transform _enemy)
    {
        Transform _sp;
        while (true)
        {  
            _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (_sp != _lastSP) break;
        }
        _lastSP = _sp;
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}
