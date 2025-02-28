using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] WaveSO[] _waves;
    private WaveSO _currentWave;
    private int _currentWaveIndex = 0;
    private WAVE_STATE _currentWaveState;
    private float _waveStartTime;
    private float _waveEndTime;
    private int _enemiesSpawned;
    private GameObject _tmpEnemy;
    private Vector2 _spawnPoint;
    private float _lastEnemySpawnedTime;

    // get rid of these and put them in a function somewhere else to be accessed globally
    float _widthHeighRatio, _adjustedScreenWidth, _cameraOrthographicSize;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (_waves.Length == 0)
        {
            Debug.LogError("NO WAVES HAVE BEEN ADDED TO THE LEVEL");
            return;
        }

        SetupWave(_waves[_currentWaveIndex]);

        _cameraOrthographicSize = Camera.main.orthographicSize;
        _widthHeighRatio = (float)Screen.width / (float)Screen.height;
        _adjustedScreenWidth = _cameraOrthographicSize * _widthHeighRatio;
    }

    private void SetupWave(WaveSO currentWave)
    {
        _currentWave = currentWave;
        _currentWaveState = WAVE_STATE.WaitingToBegin;
        _waveStartTime = Time.time;
        _enemiesSpawned = 0;
        Debug.Log("Wave " + (_currentWaveIndex + 1) + " Starting");
        Debug.Log("Wating for wave to begin");
    }

    // Update is called once per frame
    void Update()
    {
        DoWave();
    }

    void DoWave()
    {
        if (_currentWaveState == WAVE_STATE.WaitingToBegin)
        {
            if (Time.time > _waveStartTime + _currentWave.TimeBeforeWaveBegins)
            {
                Debug.Log("Wave starting");
                _currentWaveState = WAVE_STATE.Spawning;
                return;
            }
        }
        if (_currentWaveState == WAVE_STATE.Spawning)
        {
            if (_enemiesSpawned >= _currentWave.EnemyCount)
            {
                Debug.Log("Wating for wave to end");
                _currentWaveState = WAVE_STATE.WaitingToEnd;
                _waveEndTime = Time.time;

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
                if (_currentWaveIndex != _waves.Length)
                {
                    _currentWaveIndex++;
                    SetupWave(_waves[_currentWaveIndex]);
                }
                else
                {
                    Debug.Log("End of wave(s)");
                }
            }
        }
    }

    private void SpawnEnemy()
    {
        Debug.Log("Spawning enemy " + (_enemiesSpawned + 1));
        if (_currentWave.SpawnLocationType == SPAWN_LOCATION_TYPE.Range)
        {
            if (_currentWave.SpawnPositionType == SPAWN_POSITION_TYPE.Custom)
                _spawnPoint = new Vector2(Random.Range(_currentWave.CustomSpawnPointMinimum.x, _currentWave.CustomSpawnPointMaximum.x), Random.Range(_currentWave.CustomSpawnPointMinimum.y, _currentWave.CustomSpawnPointMaximum.y));
            else
                _spawnPoint = new Vector2(Random.Range(-_adjustedScreenWidth, _adjustedScreenWidth), _cameraOrthographicSize + 2f);
        }
        else _spawnPoint = _currentWave.SpawnPoint;

        _tmpEnemy = Instantiate(_currentWave.Enemy.EnemyPrefab, _spawnPoint, Quaternion.identity);
        _tmpEnemy.GetComponent<IEnemy>().SetEnemyData(_currentWave.Enemy);
        _enemiesSpawned++;
        _lastEnemySpawnedTime = Time.time;
    }
}

public enum WAVE_STATE
{
    WaitingToBegin,
    Spawning,
    WaitingToEnd
}
