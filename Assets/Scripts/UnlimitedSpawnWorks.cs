using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlimitedSpawnWorks : MonoBehaviour
{

    public enum SpawnState
    {
        SPAWNING, WAITING, COUNTING
    };
    public static string _state;

    [SerializeField]
    public int currentWave = 1;
    [SerializeField]
    public Transform enemy1;
    [SerializeField]
    public Transform enemy2;


    public class Wave
    {
        public int waveNumber;
        public Transform enemyType1;
        public Transform enemyType2;
        public int count;
        public float spawnRate;
    }

    public int NextWave
    {
        get { return currentWave + 1; } //so game doesn't start at level 0
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 10f;
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
        GameMaster.gm.onToggleStoreMenu += OnStoreMenuToggle;

        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0) Debug.LogError("No spawnpoints linked in array.");

        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError("No AudioManager found.");

        GenerateNewWave(); //starts the initial wave.
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
                Debug.Log("Wave completed.");
            }
            else return; //if enemies are alive, you don't need all the stuff below.
        }
        if (waveCountdown <= 0) //SpawnState.Counting
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(incomingWave));
            }
        }
        else
        {
            if (notInStore) waveCountdown -= Time.deltaTime;
        }
    }
    void WaveCompleted()
    {
        GenerateNewWave();

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
    }

    public Wave incomingWave = new Wave();
    void GenerateNewWave()
    {
        currentWave++;
        incomingWave.waveNumber = currentWave;
        incomingWave.enemyType1 = enemy1;
        incomingWave.enemyType2 = enemy2;
        if (currentWave <= 5) incomingWave.count = Random.Range(currentWave, 5 + currentWave);
        else if (currentWave <= 10) incomingWave.count = Random.Range(currentWave, 10 + currentWave);
        else if (currentWave > 10) incomingWave.count = 2 * currentWave;
        incomingWave.spawnRate = Random.Range((float)currentWave, currentWave * 1.5f );

        Debug.Log("Wave " + currentWave + " created");
        Debug.Log("Enemies in next wave: " + incomingWave.count);
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
        Debug.Log("Enemy being created");
    }

    bool notInStore = true;
    void OnStoreMenuToggle(bool active)
    {
        notInStore = !notInStore;
    }
}
