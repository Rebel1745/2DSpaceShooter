using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField] WaveSO[] _waves;
    private WaveSO _currentWave;
    private int _currentWaveIndex = 0;
    private WAVE_STATE _currentWaveState;
    [SerializeField] private float _waveSpawnPointYOffset = 2f;
    private float _waveStartTime;
    private float _waveEndTime;
    private EnemySO _enemy;
    private int _enemiesSpawned;
    private int _enemiesDestroyed;
    private GameObject _tmpEnemy;
    private float _spawnPointX;
    private float _lastEnemySpawnedTime;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (_waves.Length == 0)
        {
            Debug.LogError("NO WAVES HAVE BEEN ADDED TO THE LEVEL");
            return;
        }

        SetupWave(_waves[_currentWaveIndex]);
    }

    private void SetupWave(WaveSO currentWave)
    {
        _currentWave = currentWave;
        _currentWaveState = WAVE_STATE.WaitingToBegin;
        _waveStartTime = Time.time;
        _enemiesSpawned = 0;
        _enemiesDestroyed = 0;
        //Debug.Log("Wave " + (_currentWaveIndex + 1) + " Starting");
        //Debug.Log("Wating for wave to begin");
    }

    // Update is called once per frame
    void Update()
    {
        DoWave();
    }

    void DoWave()
    {
        // if we are waiting for all enemies to be destroyed, do nothing
        if (_currentWaveState == WAVE_STATE.WaitingForDestruction)
            return;

        if (_currentWaveState == WAVE_STATE.WaitingToBegin)
        {
            if (Time.time > _waveStartTime + _currentWave.TimeBeforeWaveBegins)
            {
                //Debug.Log("Wave starting");
                _currentWaveState = WAVE_STATE.Spawning;
                return;
            }
        }
        if (_currentWaveState == WAVE_STATE.Spawning)
        {
            if (_enemiesSpawned >= _currentWave.EnemyCount)
            {
                if (_currentWave.WaitForEnemiesToBeDestroyedBeforeNextWave)
                    _currentWaveState = WAVE_STATE.WaitingForDestruction;
                else
                {
                    _currentWaveState = WAVE_STATE.WaitingToEnd;
                    _waveEndTime = Time.time;
                }

                return;
            }

            // actually spawn the current enemy in the wave
            if (Time.time > _lastEnemySpawnedTime + _currentWave.TimeBetweenEnemy)
                SpawnEnemy();
        }
        if (_currentWaveState == WAVE_STATE.WaitingToEnd)
        {
            if (Time.time > _waveEndTime + _currentWave.TimeBeforeWaveEnds)
            {
                _currentWaveIndex++;
                if (_currentWaveIndex < _waves.Length)
                {
                    SetupWave(_waves[_currentWaveIndex]);
                }
                else
                {
                    //Debug.Log("End of wave(s)");
                }
            }
        }
    }

    private void SpawnEnemy()
    {
        if (_currentWave.RandomEnemy)
            _enemy = _currentWave.Enemies[Random.Range(0, _currentWave.Enemies.Length)];

        //Debug.Log("Spawning enemy " + (_enemiesSpawned + 1));
        if (_currentWave.SpawnLocationType == SPAWN_LOCATION_TYPE.Range) _spawnPointX = Random.Range((float)-GameManager.Instance.AdjustedScreenWidth, (float)GameManager.Instance.AdjustedScreenWidth);
        else _spawnPointX = _currentWave.SpawnPointX;

        _tmpEnemy = null;

        _tmpEnemy = ObjectPoolManager.SpawnObject(_enemy.EnemyPrefab, new Vector3(_spawnPointX, GameManager.Instance.CameraOrthographicSize + _waveSpawnPointYOffset, 0f), Quaternion.identity, ObjectPoolManager.POOL_TYPE.Enemy);
        _tmpEnemy.GetComponent<IEnemy>().SetEnemyData(_enemy);
        if (_enemy.RotateSpriteRandomly)
        {
            SpriteRenderer sr = _tmpEnemy.GetComponentInChildren<SpriteRenderer>();
            sr.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }
        _enemiesSpawned++;
        _lastEnemySpawnedTime = Time.time;
    }

    public void EnemyDestroyed()
    {
        _enemiesDestroyed++;

        if (_enemiesDestroyed == _enemiesSpawned)
            _currentWaveState = WAVE_STATE.WaitingToEnd;
    }
}

public enum WAVE_STATE
{
    WaitingToBegin,
    Spawning,
    WaitingForDestruction,
    WaitingToEnd
}
